﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Gender { N, T, M, F };
public enum Stat { Con, Str, Agi, Rea, Mnd, Int, Wil, Cha };
public enum UnitType { Normal, Grass };

public class Unit {

  public string name;
  public Gender gender;

  public int level, exp;
  public Dictionary<Stat, int> stats;
  public List<UnitType> type;
  public int healthCur, healthMax, manaCur, manaMax;

  public List<Action> actions;
  public List<Passive> passives;

  public Unit() {
    name = "Boi";
    gender = Gender.M;

    level = 1;
    exp = 0;

    stats = new Dictionary<Stat, int>();
    stats.Add(Stat.Con, 5);
    stats.Add(Stat.Str, 5);
    stats.Add(Stat.Agi, 5);
    stats.Add(Stat.Rea, 5);
    stats.Add(Stat.Mnd, 5);
    stats.Add(Stat.Int, 5);
    stats.Add(Stat.Wil, 5);
    stats.Add(Stat.Cha, 5);

    healthMax = 80 + stats[Stat.Con] * 7 + stats[Stat.Str] * 3 + stats[Stat.Agi] * 2 + level;
    healthCur = healthMax;
    manaMax = 10 + stats[Stat.Mnd] * 7 + stats[Stat.Int] * 3 + stats[Stat.Wil] * 2 + level;
    manaCur = manaMax;

    type = new List<UnitType>();
    type.Add(UnitType.Grass);
    type.Add(UnitType.Normal);

    actions = new List<Action>();
    passives = new List<Passive>();
  }

  public override string ToString() {
    return name + " " + gender + " Lvl." + level;
  }
}