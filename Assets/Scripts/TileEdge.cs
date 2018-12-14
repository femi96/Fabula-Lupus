using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEdge {

  private TileNode destination;
  private float weight;

  public TileEdge(TileNode n, float w) {
    destination = n;
    weight = w;
  }

  public TileNode GetNode() {
    return destination;
  }

  public float GetWeight() {
    return weight;
  }

  public override string ToString() {
    return "Edge to " + destination.GetKey().ToString() + " for " + weight.ToString();

  }
}
