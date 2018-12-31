using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit {

  public Unit unit;
  public Vector3 position;
  public Vector2Int key;
  public int team;
  public GameObject body;

  public BattleUnit(Unit u, Vector2Int k, int t, GameObject g) {
    unit = u;
    key = k;
    team = t;
    body = g;
    position = body.transform.position;
  }

  public override string ToString() {
    return unit.ToString();
  }

  // public Vector2Int GetPosKey() {}

  public void SetPos(Vector3 pos, Vector2Int k) {
    body.transform.position = pos;
    position = pos;
    key = k;
  }
}
