using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitClassBonus {

  private int level;
  private bool isStat = false, isAction = false, isPassive = false;
  private Stat stat;
  private int amount;
  private Action action;
  private Passive passive;

  public UnitClassBonus(int lvl, Stat s, int a) {
    level = lvl;
    isStat = true;
    stat = s;
    amount = a;
  }

  public UnitClassBonus(int lvl, Action a) {
    level = lvl;
    isAction = true;
    action = a;
  }

  public UnitClassBonus(int lvl, Passive p) {
    level = lvl;
    isPassive = true;
    passive = p;
  }

  public void ApplyToUnit(Unit unit) {

    if (unit.level < level)
      return;

    if (isStat)
      unit.stats[stat] += amount;

    if (isAction)
      unit.actions.Add(action);

    if (isPassive)
      unit.passives.Add(passive);
  }
}