using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit {

  public Unit unit;
  public Vector2Int position;
  public int team;
  public GameObject body;

  public BattleUnit(Unit u, Vector2Int p, int t, GameObject g) {
    unit = u;
    position = p;
    team = t;
    body = g;
  }

  public override string ToString() {
    return unit.ToString();
  }

  public void SetPos(Vector3 pos, Vector2Int key) {
    body.transform.position = pos;
    position = key;
  }
}
