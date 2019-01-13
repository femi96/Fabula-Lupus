using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dweller : UnitClass {

  public override List<UnitClassBonus> GetBonuses() {
    return new List<UnitClassBonus>() {
      new UnitClassBonus(1, Stat.Con, 1),
    };
  }

  public override string GetName() {
    return "Dweller";
  }
}