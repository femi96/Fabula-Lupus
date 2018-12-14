using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour {

  void Start() {
    GenerateGraph();
  }

  void Update() {}

  private void GenerateGraph() {
    Dictionary<Vector2Int, TileNode> dict = new Dictionary<Vector2Int, TileNode>();

    for (int i = 0; i < 8; i++) {
      for (int j = 0; j < 8; j++) {
        TileNode node = new TileNode(new Vector3(i, (i + j) / 10f, j));
        dict.Add(node.GetKey(), node);
      }
    }

    foreach (KeyValuePair<Vector2Int, TileNode> entry in dict) {
      Vector2Int k = entry.Key;
      TileNode n = entry.Value;

      for (int i = k.x - 1; i <= k.x + 1; i++) {
        for (int j = k.y - 1; j <= k.y + 1; j++) {

          if (i == k.x && j == k.y)
            continue;

          Vector2Int t = new Vector2Int(i, j);

          if (!dict.ContainsKey(t))
            continue;

          n.AddNeighbor(dict[t], (n.GetPos() - dict[t].GetPos()).magnitude);
        }
      }
    }

    Debug.Log(dict[new Vector2Int(0, 0)]);
  }
}
