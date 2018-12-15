using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit {

  public Unit unit;
  public Vector2Int position;
  public int team;

  public BattleUnit(Unit u, Vector2Int p, int t) {
    unit = u;
    position = p;
    team = t;
  }

  public override string ToString() {
    return unit.ToString();
  }
}
