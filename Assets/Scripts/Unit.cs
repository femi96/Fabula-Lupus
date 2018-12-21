using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Gender { N, K, S, X, M, F };
public enum Stat { Con, Str, Agi, Rea, Mnd, Int, Wil, Cha };
public enum UnitType { Normal, Grass };

public class Unit {

  public string name;
  public Gender gender;
  public string cls;

  public int level, exp;
  public Dictionary<Stat, int> stats;
  public List<UnitType> type;
  public float move;
  public float jump;
  public int healthCur, healthMax, manaCur, manaMax, speedCur, speedMax, speedTemp;

  public List<Action> actions;
  public List<Passive> passives;

  public Unit() {
    name = "Boi " + Random.Range(1, 100).ToString();
    gender = Gender.N;
    cls = "Ravager";

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

    move = 2f;
    jump = 0.5f;

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

  /* Unit UI */
  public void SetPanelUI(GameObject panelUI) {
    Transform panel = panelUI.transform;
    panel.Find("BackImage/NameText").gameObject.GetComponent<Text>().text = name;
    panel.Find("Level/LevelText").gameObject.GetComponent<Text>().text = level.ToString();
    panel.Find("Health/HealthText").gameObject.GetComponent<Text>().text = healthCur.ToString() + "/" + healthMax.ToString();
    panel.Find("Mana/ManaText").gameObject.GetComponent<Text>().text = manaCur.ToString() + "/" + manaMax.ToString();
  }

  public void SetStatusUI(GameObject statusUI) {
    Transform screen = statusUI.transform.Find("BackImage");
    screen.Find("NameText").gameObject.GetComponent<Text>().text = name;
    screen.Find("ClassText").gameObject.GetComponent<Text>().text = cls;
    screen.Find("GenderText").gameObject.GetComponent<Text>().text = gender.ToString();

    for (int i = 1; i <= 3; i++) {
      if (type.Count < i)
        screen.Find("Type/Type" + i.ToString()).gameObject.GetComponent<Text>().text = "";
      else
        screen.Find("Type/Type" + i.ToString()).gameObject.GetComponent<Text>().text = type[i - 1].ToString();
    }

    screen.Find("Level/LevelText").gameObject.GetComponent<Text>().text = level.ToString();
    screen.Find("Exp/ExpText").gameObject.GetComponent<Text>().text = exp.ToString();
    screen.Find("Health/HealthText").gameObject.GetComponent<Text>().text = healthCur.ToString() + "/" + healthMax.ToString();
    screen.Find("Mana/ManaText").gameObject.GetComponent<Text>().text = manaCur.ToString() + "/" + manaMax.ToString();

    screen.Find("Move/MoveText").gameObject.GetComponent<Text>().text = move.ToString();
    screen.Find("Jump/JumpText").gameObject.GetComponent<Text>().text = jump.ToString();
    screen.Find("Speed/SpeedText").gameObject.GetComponent<Text>().text = speedCur.ToString();

    screen.Find("Con/ConText").gameObject.GetComponent<Text>().text = stats[Stat.Con].ToString();
    screen.Find("Str/StrText").gameObject.GetComponent<Text>().text = stats[Stat.Str].ToString();
    screen.Find("Agi/AgiText").gameObject.GetComponent<Text>().text = stats[Stat.Agi].ToString();
    screen.Find("Rea/ReaText").gameObject.GetComponent<Text>().text = stats[Stat.Rea].ToString();
    screen.Find("Mnd/MndText").gameObject.GetComponent<Text>().text = stats[Stat.Mnd].ToString();
    screen.Find("Int/IntText").gameObject.GetComponent<Text>().text = stats[Stat.Int].ToString();
    screen.Find("Wil/WilText").gameObject.GetComponent<Text>().text = stats[Stat.Wil].ToString();
    screen.Find("Cha/ChaText").gameObject.GetComponent<Text>().text = stats[Stat.Cha].ToString();
  }
}