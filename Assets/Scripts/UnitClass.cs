﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class UnitClass {

  public int level = 0;
  public int exp = 0;

  public abstract List<UnitClassBonus> GetBonuses();

  public void AddExp(int expDelta) {

    if (level == 10) {
      exp = 0;
      return;
    }

    exp += expDelta;

    // Level up
    while (exp >= GetMaxExp()) {

      if (level == 10) {
        exp = 0;
        return;
      }

      exp -= GetMaxExp();
      level += 1;
    }
  }

  public int GetMaxExp() {
    return level * 3 + 6;
  }

  public void ApplyBonusToUnit(Unit unit) {
    List<UnitClassBonus> bonuses = GetBonuses();

    foreach (UnitClassBonus bonus in bonuses) {
      bonus.ApplyToUnit(unit);
    }
  }

  public void SetClassUI() {
    // None for now
  }

  public override string ToString() {
    if (level == 10)
      return GetName() + ".X";

    return GetName() + "." + level;
  }

  public abstract string GetName();
}