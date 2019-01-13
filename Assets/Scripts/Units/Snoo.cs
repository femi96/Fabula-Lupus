using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snoo : UnitGenerator {

  public override Unit NewUnit() {
    Unit unit = new Unit();

    unit.name = "Snoo";
    unit.gender = Gender.M;
    unit.race = new Hume();
    unit.classes = new List<UnitClass>();
    unit.classes.Add(new Doodle());
    unit.classes[0].AddExp(10000);

    Dictionary<Stat, int> statsUnique = new Dictionary<Stat, int>();
    statsUnique.Add(Stat.Con, -1);
    statsUnique.Add(Stat.Str, 0);
    statsUnique.Add(Stat.Agi, 0);
    statsUnique.Add(Stat.Rea, 1);
    statsUnique.Add(Stat.Mnd, 0);
    statsUnique.Add(Stat.Int, 1);
    statsUnique.Add(Stat.Wil, 1);
    statsUnique.Add(Stat.Cha, 1);

    unit.statsUnique = statsUnique;

    // unit.bodyResource = "SnooBody";
    // unit.profileResource = "SnooProfile";

    unit.UpdateDerivedStats();
    unit.ResetCurrentStats();

    return unit;
  }
}