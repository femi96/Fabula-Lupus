using System.Collections;
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
  public int healthCur, healthMax, manaCur, manaMax, speedCur, speedMax, speedTemp;

  public List<Action> actions;
  public List<Passive> passives;

  public Unit() {
    name = "Boi " + Random.Range(1, 100).ToString();
    gender = Gender.M;

    level = Random.Range(1, 11);
    exp = 0;

    stats = new Dictionary<Stat, int>();
    stats.Add(Stat.Con, Random.Range(3, 8));
    stats.Add(Stat.Str, Random.Range(3, 8));
    stats.Add(Stat.Agi, Random.Range(3, 8));
    stats.Add(Stat.Rea, Random.Range(3, 8));
    stats.Add(Stat.Mnd, Random.Range(3, 8));
    stats.Add(Stat.Int, Random.Range(3, 8));
    stats.Add(Stat.Wil, Random.Range(3, 8));
    stats.Add(Stat.Cha, Random.Range(3, 8));

    healthMax = 40 + stats[Stat.Con] * 3 + stats[Stat.Str];
    healthCur = healthMax;
    manaMax = 10 + stats[Stat.Mnd] * 3 + stats[Stat.Int];
    manaCur = manaMax;
    speedMax = stats[Stat.Rea] + stats[Stat.Int];
    speedCur = speedMax;
    speedTemp = speedCur;

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