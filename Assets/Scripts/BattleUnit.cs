using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit {

  public Unit unit;
  public int team;
  private Vector2Int key;
  private GameObject body;

  public BattleUnit(Unit u, Vector2Int k, int t, GameObject g) {
    unit = u;
    key = k;
    team = t;
    body = g;
  }

  public override string ToString() {
    return unit.ToString();
  }

  public Vector2Int GetPosKey() {
    return key;
  }

  public Vector3 GetPos() {
    return body.transform.position;;
  }

  public void SetPos(Vector3 pos, Vector2Int k) {
    body.transform.position = pos;
    key = k;
  }
}
