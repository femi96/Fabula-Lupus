using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keaton : UnitGenerator {

  public override Unit NewUnit() {
    Unit unit = new Unit();

    unit.name = "Keaton";
    unit.gender = Gender.M;
    unit.race = new Hume();
    unit.classes = new List<UnitClass>();
    unit.classes.Add(new Knight());

    Dictionary<Stat, int> statsUnique = new Dictionary<Stat, int>();
    statsUnique.Add(Stat.Con, 1);
    statsUnique.Add(Stat.Str, 2);
    statsUnique.Add(Stat.Agi, 0);
    statsUnique.Add(Stat.Rea, 0);
    statsUnique.Add(Stat.Mnd, -1);
    statsUnique.Add(Stat.Int, -1);
    statsUnique.Add(Stat.Wil, 2);
    statsUnique.Add(Stat.Cha, 0);

    unit.statsUnique = statsUnique;

    unit.bodyResource = "KeatBody";
    unit.profileResource = "KeatProfile";

    unit.UpdateDerivedStats();
    unit.ResetCurrentStats();

    return unit;
  }
}