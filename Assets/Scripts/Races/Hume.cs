using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hume : UnitRace {

  public override void SetBaseStats(Unit unit) {
    Dictionary<Stat, int> stats = new Dictionary<Stat, int>();
    stats.Add(Stat.Con, 3);
    stats.Add(Stat.Str, 3);
    stats.Add(Stat.Agi, 3);
    stats.Add(Stat.Rea, 3);
    stats.Add(Stat.Mnd, 3);
    stats.Add(Stat.Int, 3);
    stats.Add(Stat.Wil, 3);
    stats.Add(Stat.Cha, 3);

    unit.stats = stats;
    unit.move = 2f;
    unit.jump = 0.5f;
  }
}
