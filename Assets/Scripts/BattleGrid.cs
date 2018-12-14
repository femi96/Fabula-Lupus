using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGrid : MonoBehaviour {

  void Start() {
    GenerateGraphFromGameObject();
  }

  void Update() {}

  private Dictionary<Vector2Int, TileNode> tileDict;

  private void GenerateGraphFromGameObject() {
    tileDict = new Dictionary<Vector2Int, TileNode>();

    // Add nodes from gameobject children
    foreach (Transform child in transform) {
      TileNode node = new TileNode(child.position);
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

          n.AddNeighbor(tileDict[t], (n.GetPos() - tileDict[t].GetPos()).magnitude);
        }
      }
    }
  }
}
