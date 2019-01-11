using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Knight : UnitClass {

  public override Dictionary<int, UnitClassBonus> GetBonuses() {
    return new Dictionary<int, UnitClassBonus>() {
      { 1, new UnitClassBonus(Stat.Con, 1) },
      { 5, new UnitClassBonus(Stat.Str, 10) },
      { 10, new UnitClassBonus(Stat.Wil, 11) },
    };
  }
}
