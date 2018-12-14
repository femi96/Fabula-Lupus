using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Gender { N, T, M, F };
public enum Stat { Con, Str, Agi, Rea, Mnd, Int, Wil, Cha };
public enum UnitType { Normal, Grass };

public class Unit {

  private string name;
  private Gender gender;

  private Dictionary<Stat, int> stats;
  private List<UnitType> type;

  private List<Action> actions;
  private List<Passive> passives;

  public Unit() {}
}