﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGrid : MonoBehaviour {

  void Start() {
    GenerateGraphFromGameObject();
    AddUnits();
  }

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

      if (entry.Value == 0)
        unit = Snoo.NewUnit();

      if (entry.Value == 1)
        unit = Keaton.NewUnit();

      GameObject go = Instantiate(unit.GetBody(), tileDict[entry.Key].GetPos(), Quaternion.identity, transform);
      units.Add(new BattleUnit(unit, entry.Key, entry.Value, go));
    }
  }

  public int GetEndState() {
    bool alliesAlive = false;
    bool enemiesAlive = false;

    foreach (BattleUnit bu in units) {
      if (bu.unit.IsAlive()) {
        if (bu.team == 0)
          alliesAlive = true;

        if (bu.team == 1)
          enemiesAlive = true;
      }
    }

    if (!alliesAlive)
      return 1;

    if (!enemiesAlive)
      return 0;

    return -1; // Ongoing
  }

  public void NextCurrentUnit() {
    if (unitQueue == null || unitQueue.Count == 0)
      UpdateUnitQueue();

    currentUnit = unitQueue[0];
    currentUnit.unit.apCur = currentUnit.unit.apMax;
    unitQueue.RemoveAt(0);

    if (!currentUnit.unit.IsAlive())
      NextCurrentUnit();
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

  public bool isMovingUnit = false;
  private BattleUnit movingUnit;
  private List<TileNode> movingPath;
  private float moveTime = 0f;

  private TileNode moveSrc;
  private TileNode moveDest;

  public bool isActingUnit = false;

  public bool isControllingUnit = false;
  public float controlTime = 0f;

  public void MoveUnit(BattleUnit unit, List<TileNode> path) {
    isMovingUnit = true;
    movingUnit = unit;
    movingPath = path;
    moveTime = 1f;

    moveSrc = path[0];
    moveDest = moveSrc;
    movingPath.RemoveAt(0);
    movingUnit.unit.apCur -= 1;
  }

  public void ActUnit(BattleUnit unit, Action action, TileNode target) {
    List<TileNode> targets = new List<TileNode>();
    targets.Add(target);
    ActUnit(unit, action, targets);
  }

  public void ActUnit(BattleUnit unit, Action action, List<TileNode> targets) {
    foreach (TileNode target in targets) {
      action.ApplyAction(target, this);
    }

    unit.unit.apCur -= 1;
  }

  public bool ShowPanel() {
    return !(isMovingUnit || isActingUnit);
  }

  void Update() {
    if (isMovingUnit) {
      moveTime += Time.deltaTime * 2.6f;

      if (moveTime >= 1f) {
        movingUnit.body.transform.position = moveDest.GetPos();
        movingUnit.body.transform.GetChild(0).rotation = movingUnit.GetFaceRotation();
        movingUnit.body.transform.GetChild(0).localPosition = 0.8f * Vector3.up;
        movingUnit.SetPosKey(moveDest.GetKey());

        if (movingPath.Count == 0) {
          isMovingUnit = false;
        } else {

          moveSrc = moveDest;
          moveDest = movingPath[0];
          movingPath.RemoveAt(0);
          moveTime = 0f;

          Vector3 moveDir = (moveDest.GetPos() - moveSrc.GetPos()).normalized;
          Vector2Int faceDir = new Vector2Int(Mathf.RoundToInt(moveDir.x), Mathf.RoundToInt(moveDir.z));
          movingUnit.SetFaceDirection(faceDir);
        }

      } else {
        // Vector3 pos = moveSrc.GetPos() * (1 - moveTime) + moveDest.GetPos() * moveTime;
        // movingUnit.body.transform.position = pos;

        Vector3 moveDir = (moveDest.GetPos() - moveSrc.GetPos()).normalized;

        float stepSize = 1f / 4f;

        float stepTime = Mathf.FloorToInt(moveTime / stepSize) * stepSize;
        // stepTime = moveTime;
        float rotAngle = stepTime * 360;
        Vector3 rotVec = Vector3.Cross(Vector3.up, Vector3.forward);
        Vector3 pos = moveSrc.GetPos() * (1 - stepTime) + moveDest.GetPos() * stepTime;
        movingUnit.body.transform.position = pos;
        movingUnit.body.transform.GetChild(0).rotation = movingUnit.GetFaceRotation() * Quaternion.AngleAxis(rotAngle, rotVec);
        movingUnit.body.transform.GetChild(0).localPosition = (Mathf.Abs(Mathf.Cos(rotAngle * Mathf.Deg2Rad)) * 0.4f + 0.4f) * Vector3.up;

        // movingUnit.SetPosKey(moveDest.GetKey());
      }
    }

    if (isControllingUnit) {
      controlTime += Time.deltaTime;

      if (currentUnit.unit.apCur == 2 && controlTime >= 1f) {
        HashSet<TileNode> targets = GetMoveTiles(currentUnit);
        TileNode end = targets.ToArray()[UnityEngine.Random.Range(0, targets.Count)];
        List<TileNode> path = GetMovePath(currentUnit, end);
        MoveUnit(currentUnit, path);
        controlTime -= 1f;
      }

      if (currentUnit.unit.apCur == 1 && !isMovingUnit && controlTime >= 1f) {
        List<Action> actions = currentUnit.unit.actions;
        Action action = actions[UnityEngine.Random.Range(0, actions.Count)];

        HashSet<TileNode> targets = action.GetSelectionTiles(currentUnit, this);

        if (action.GetActionType() == ActionType.Targeted) {
          TileNode target = targets.ToArray()[UnityEngine.Random.Range(0, targets.Count)];
          ActUnit(currentUnit, action, target);
        }

        if (action.GetActionType() == ActionType.Fixed) {
          List<TileNode> target = new List<TileNode>(targets.ToArray());
          ActUnit(currentUnit, action, target);
        }

        controlTime -= 1f;
      }

      if (currentUnit.unit.apCur == 0 && !isActingUnit && controlTime >= 1f) {
        isControllingUnit = false;
        controlTime -= 1f;
      }
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
      if (unit.GetPosKey() == k)
        return unit;

    return null;
  }

  public TileNode GetTile(Vector3 v) {
    Vector2Int k = Vector3ToKey(v);
    return GetTile(k);
  }

  public TileNode GetTile(Vector2Int k) {
    if (tileDict.ContainsKey(k))
      return tileDict[k];

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

    distances[tileDict[bu.GetPosKey()]] = 0f;
    queue.Add(tileDict[bu.GetPosKey()]);

    // Start search
    while (queue.Count != 0) {
      queue = queue.OrderBy(node => distances[node]).ToList();
      TileNode current = queue[0];
      queue.RemoveAt(0);
      visited.Add(current);

      float baseDist = distances[current];
      float baseHeight = current.GetPos().y;

      foreach (TileEdge edge in current.edges) {
        float edgeDist = baseDist + edge.GetWeight();
        TileNode node = edge.GetNode();
        float height = Mathf.Abs(node.GetPos().y - baseHeight);

        // Unit blocking
        BattleUnit blockingUnit = GetUnit(node.GetPos());
        bool blocked = (blockingUnit != null && blockingUnit.team != bu.team);

        if (edgeDist <= move && edgeDist <= distances[node] && height <= jump && !blocked) {
          if (!previous.ContainsKey(node))
            queue.Add(node);

          previous[node] = current;
          distances[node] = edgeDist;
        }
      }
    }

    // Remove units and return
    foreach (BattleUnit unit in units)
      visited.Remove(tileDict[unit.GetPosKey()]);

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

    distances[tileDict[bu.GetPosKey()]] = 0f;
    queue.Add(tileDict[bu.GetPosKey()]);

    // Start search
    while (queue.Count != 0) {
      queue = queue.OrderBy(node => distances[node]).ToList();
      TileNode current = queue[0];
      queue.RemoveAt(0);

      // Exit search when hit end
      if (current == end)
        break;

      float baseDist = distances[current];
      float baseHeight = current.GetPos().y;

      foreach (TileEdge edge in current.edges) {
        float edgeDist = baseDist + edge.GetWeight();
        TileNode node = edge.GetNode();
        float height = Mathf.Abs(node.GetPos().y - baseHeight);

        // Unit blocking
        BattleUnit blockingUnit = GetUnit(node.GetPos());
        bool blocked = (blockingUnit != null && blockingUnit.team != bu.team);

        if (edgeDist <= move && edgeDist <= distances[node] && height <= jump && !blocked) {
          if (!previous.ContainsKey(node))
            queue.Add(node);

          previous[node] = current;
          distances[node] = edgeDist;
        }
      }
    }

    // Remove initial and return
    List<TileNode> path = new List<TileNode>();
    TileNode start = tileDict[bu.GetPosKey()];
    TileNode prev = end;

    while (prev != start) {
      path.Insert(0, prev);
      prev = previous[prev];
    }

    path.Insert(0, start);
    return path;
  }

  public HashSet<TileNode> GetRangeTiles(TileNode center, float range) {
    return GetRangeTiles(center, range, false, true);
  }

  public HashSet<TileNode> GetRangeTiles(TileNode center, float range, bool selfTarget, bool ignoreHeight) {
    // Use Dijkstra's alg to get tiles in range

    // Return set
    HashSet<TileNode> visited = new HashSet<TileNode>();
    range += 0.01f;

    Vector3 centerPos = center.GetPos();
    Vector3 delta;

    foreach (TileNode node in tileDict.Values) {
      delta = node.GetPos() - centerPos;

      if (ignoreHeight)
        delta = delta - delta.y * Vector3.up;

      if (delta.magnitude <= range)
        visited.Add(node);
    }

    if (!selfTarget)
      visited.Remove(center);

    return visited;
  }
}