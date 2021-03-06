﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Knight : UnitClass {

  public override List<UnitClassBonus> GetBonuses() {
    return new List<UnitClassBonus>() {
      new UnitClassBonus(1, Stat.Cha, 1),
          new UnitClassBonus(4, Stat.Con, 1),
          new UnitClassBonus(8, Stat.Agi, 2),
          new UnitClassBonus(10, Stat.Str, 2),
    };
  }

  public override string GetName() {
    return "Knight";
  }
}
