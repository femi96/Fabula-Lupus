using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitClassBonus {

  private bool isStat = false, isAction = false, isPassive = false;
  private Stat stat;
  private int amount;
  private Action action;
  private Passive passive;

  public UnitClassBonus() {}

  public UnitClassBonus(Stat s, int a) {
    isStat = true;
    stat = s;
    amount = a;
  }

  public UnitClassBonus(Action a) {
    isAction = true;
    action = a;
  }

  public UnitClassBonus(Passive p) {
    isPassive = true;
    passive = p;
  }

  public void ApplyToUnit(Unit unit) {
    if (isStat)
      unit.stats[stat] += amount;

    if (isAction)
      unit.actions.Add(action);

    if (isPassive)
      unit.passives.Add(passive);
  }
}