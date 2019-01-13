using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hume : UnitRace {

  public override void SetUnit(Unit unit) {
    foreach (Stat stat in(Stat[]) Enum.GetValues(typeof(Stat)))
      unit.stats[stat] += 3;

    unit.move = 8f;
    unit.jump = 0.5f;

    unit.type.Add(UnitType.Normal);
    unit.actions.Add(new Slap());
  }
}
