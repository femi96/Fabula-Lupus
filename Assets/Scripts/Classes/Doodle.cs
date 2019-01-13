using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Doodle : UnitClass {

  public override List<UnitClassBonus> GetBonuses() {
    return new List<UnitClassBonus>() {
      new UnitClassBonus(1, Stat.Cha, 1),
          new UnitClassBonus(2, new Snooze()),
          new UnitClassBonus(3, new Stomp()),
          new UnitClassBonus(5, Stat.Con, 1),
          new UnitClassBonus(9, Stat.Agi, 2),
          new UnitClassBonus(10, Stat.Cha, 2),
    };
  }

  public override string GetName() {
    return "Doodle";
  }
}
