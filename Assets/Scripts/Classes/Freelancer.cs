using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Freelancer : UnitClass {

  public override Dictionary<int, UnitClassBonus> GetBonuses() {
    return new Dictionary<int, UnitClassBonus>() {
      { 1, new UnitClassBonus(Stat.Con, 1) },
    };
  }
}
