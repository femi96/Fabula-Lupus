﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class UnitClass {

  public int level = 0;
  public int exp = 0;

  public Dictionary<int, UnitClassBonus> bonusDict;

  public abstract Dictionary<int, UnitClassBonus> GetBonuses();

  public void AddExp(int expDelta) {

    if (level == 10) {
      exp = 0;
      return;
    }

    exp += expDelta;

    // Level up
    if (exp >= GetMaxExp()) {
      level += 1;
      exp = exp % GetMaxExp();
    }
  }

  public int GetMaxExp() {
    return level * 3 + 6;
  }

  public void ApplyBonusToUnit(Unit unit) {
    for (int i = 0; i <= level; i++) {
      GetBonus(i).ApplyToUnit(unit);
    }
  }

  public UnitClassBonus GetBonus(int level) {
    if (bonusDict == null)
      bonusDict = GetBonuses();

    if (bonusDict.ContainsKey(level))
      return bonusDict[level];

    return new UnitClassBonus();
  }

  public void SetClassUI() {
    // None for now
  }
}