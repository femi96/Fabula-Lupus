using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileNode {

  private Vector3 pos;
  public List<TileEdge> edges;
  private Vector2Int key;

  public TileNode(Vector3 v) {
    pos = v;
    edges = new List<TileEdge>();
    key = new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.z));
  }

  public void AddNeighbor(TileNode node, float weight) {
    edges.Add(new TileEdge(node, weight));
  }

  public Vector3 GetPos() {
    return pos;
  }

  public Vector2Int GetKey() {
    return key;
  }

  public override string ToString() {
    string s = "Node " + key.ToString() + " :\n";

    foreach (TileEdge e in edges)
      s += e.ToString() + "\n";

    return s;
  }
}
