using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGrid : MonoBehaviour {

  void Start() {
    GenerateGraphFromGameObject();
    AddUnits();
  }

  void Update() {}

  private List<BattleUnit> units;

  private void AddUnits() {
    units = new List<BattleUnit>();

    foreach (KeyValuePair<Vector2Int, int> entry in spawnDict) {
      Unit unit = new Unit();
      units.Add(new BattleUnit(unit, entry.Key, entry.Value));
    }
  }

  private Dictionary<Vector2Int, TileNode> tileDict;
  private Dictionary<Vector2Int, int> spawnDict;

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
          spawnDict.Add(new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.z)), marker.val);
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
}
