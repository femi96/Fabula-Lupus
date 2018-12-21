using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGrid : MonoBehaviour {

  void Start() {
    GenerateGraphFromGameObject();
    AddUnits();
  }

  void Update() {}


  /* Units */
  private List<BattleUnit> units;
  private List<BattleUnit> unitQueue;
  [Header("Units")]
  public GameObject unitPrefab;
  public BattleUnit currentUnit;

  private void AddUnits() {
    units = new List<BattleUnit>();

    foreach (KeyValuePair<Vector2Int, int> entry in spawnDict) {
      Unit unit = new Unit();
      GameObject go = Instantiate(unitPrefab, tileDict[entry.Key].GetPos(), Quaternion.identity, transform);
      units.Add(new BattleUnit(unit, entry.Key, entry.Value, go));
    }
  }

  public void NextCurrentUnit() {
    if (unitQueue == null || unitQueue.Count == 0)
      UpdateUnitQueue();

    currentUnit = unitQueue[0];
    unitQueue.RemoveAt(0);
  }

  private void UpdateUnitQueue() {
    unitQueue = new List<BattleUnit>();

    foreach (BattleUnit u in units) {
      u.unit.speedTemp = u.unit.speedCur + UnityEngine.Random.Range(1, 7) + UnityEngine.Random.Range(1, 7);
    }

    while (true) {
      int highestSpeed = 0;
      BattleUnit highestUnit = null;

      foreach (BattleUnit u in units) {
        if (u.unit.speedTemp > highestSpeed) {
          highestSpeed = u.unit.speedTemp;
          highestUnit = u;
        }
      }

      if (highestSpeed == 0)
        break;

      unitQueue.Add(highestUnit);
      highestUnit.unit.speedTemp -= 8;
    }
  }

  /* Tiles */
  private Dictionary<Vector2Int, TileNode> tileDict;
  private Dictionary<Vector2Int, int> spawnDict;

  // Add boundaries
  [HideInInspector]
  public float xMin = 0, xMax = 0, zMin = 0, zMax = 0;

  private void GenerateGraphFromGameObject() {
    tileDict = new Dictionary<Vector2Int, TileNode>();
    spawnDict = new Dictionary<Vector2Int, int>();

    // Add nodes from gameObject children
    foreach (Transform child in transform) {
      BattleGridMarker marker = child.gameObject.GetComponent<BattleGridMarker>();
      Vector3 v = child.position;

      // If marker, add as special
      if (marker) {
        switch (marker.type) {
        case MarkerType.Spawn:
          spawnDict.Add(Vector3ToKey(v), marker.val);
          break;

        default:
          Debug.Log("Should not reach here");
          break;
        }

        child.gameObject.SetActive(false);
        continue;
      }

      // Else, add as tile
      TileNode node = new TileNode(v);
      tileDict.Add(node.GetKey(), node);

      // Update boundaries
      xMin = Mathf.Min(xMin, v.x - 0.45f);
      xMax = Mathf.Max(xMax, v.x + 0.45f);
      zMin = Mathf.Min(zMin, v.z - 0.45f);
      zMax = Mathf.Max(zMax, v.z + 0.45f);
    }

    // Add edges for all adjacent nodes
    foreach (KeyValuePair<Vector2Int, TileNode> entry in tileDict) {
      Vector2Int k = entry.Key;
      TileNode n = entry.Value;

      for (int i = k.x - 1; i <= k.x + 1; i++) {
        for (int j = k.y - 1; j <= k.y + 1; j++) {

          if (i == k.x && j == k.y)
            continue;

          Vector2Int t = new Vector2Int(i, j);

          if (!tileDict.ContainsKey(t))
            continue;

          float w = (n.GetPos() - tileDict[t].GetPos()).magnitude;
          w = Mathf.RoundToInt(w * 10) / 10f;
          n.AddNeighbor(tileDict[t], w);
        }
      }
    }

    // Debug printing
    /*
    foreach (KeyValuePair<Vector2Int, TileNode> entry in tileDict) {
      Debug.Log(entry.Value);
    }
    foreach (KeyValuePair<Vector2Int, int> entry in spawnDict) {
      Debug.Log(entry.Value);
    }
    */
  }

  private Vector2Int Vector3ToKey(Vector3 v) {
    return new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.z));
  }

  public float GetHeight(Vector3 v) {
    Vector2Int k = Vector3ToKey(v);

    if (tileDict.ContainsKey(k))
      return tileDict[k].GetPos().y;

    return 0f;

  }

  public BattleUnit GetUnit(Vector3 v) {
    Vector2Int k = Vector3ToKey(v);

    foreach (BattleUnit unit in units)
      if (unit.position == k)
        return unit;

    return null;
  }

  public HashSet<TileNode> GetMoveTiles(BattleUnit bu) {
    // Use Dijkstra's alg to get tiles in range
    float move = bu.unit.move;
    float jump = bu.unit.jump;

    // Return set
    HashSet<TileNode> visited = new HashSet<TileNode>();

    // Initial collection values
    List<TileNode> queue = new List<TileNode>();
    Dictionary<TileNode, TileNode> previous = new Dictionary<TileNode, TileNode>();
    Dictionary<TileNode, float> distances = new Dictionary<TileNode, float>();

    foreach (TileNode node in tileDict.Values) {
      distances.Add(node, float.MaxValue);
    }

    distances[tileDict[bu.position]] = 0f;
    queue.Add(tileDict[bu.position]);

    // Start search
    while (queue.Count != 0) {
      queue = queue.OrderBy(node => distances[node]).ToList();
      TileNode current = queue[0];
      queue.RemoveAt(0);
      visited.Add(current);

      float baseDist = distances[current];

      foreach (TileEdge edge in current.edges) {
        float edgeDist = baseDist + edge.GetWeight();
        TileNode node = edge.GetNode();

        if (edgeDist <= move && edgeDist <= distances[node]) {
          if (!previous.ContainsKey(node))
            queue.Add(node);

          previous[node] = current;
          distances[node] = edgeDist;
        }
      }
    }

    // Remove initial and return
    visited.Remove(tileDict[bu.position]);
    return visited;
  }

  public List<TileNode> GetMovePath(BattleUnit bu, TileNode end) {
    // Use Dijkstra's alg to get tiles in range
    float move = bu.unit.move;
    float jump = bu.unit.jump;

    // Initial collection values
    List<TileNode> queue = new List<TileNode>();
    Dictionary<TileNode, TileNode> previous = new Dictionary<TileNode, TileNode>();
    Dictionary<TileNode, float> distances = new Dictionary<TileNode, float>();

    foreach (TileNode node in tileDict.Values) {
      distances.Add(node, float.MaxValue);
    }

    distances[tileDict[bu.position]] = 0f;
    queue.Add(tileDict[bu.position]);

    // Start search
    while (queue.Count != 0) {
      queue = queue.OrderBy(node => distances[node]).ToList();
      TileNode current = queue[0];
      queue.RemoveAt(0);

      // Exit search when hit end
      if (current == end)
        break;

      float baseDist = distances[current];

      foreach (TileEdge edge in current.edges) {
        float edgeDist = baseDist + edge.GetWeight();
        TileNode node = edge.GetNode();

        if (edgeDist <= move && edgeDist <= distances[node]) {
          if (!previous.ContainsKey(node))
            queue.Add(node);

          previous[node] = current;
          distances[node] = edgeDist;
        }
      }
    }

    // Remove initial and return
    List<TileNode> path = new List<TileNode>();
    TileNode start = tileDict[bu.position];
    TileNode prev = end;

    while (prev != start) {
      path.Insert(0, prev);
      prev = previous[prev];
    }

    path.Insert(0, start);
    return path;
  }
}