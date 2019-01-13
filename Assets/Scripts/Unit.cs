using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Gender { N, K, S, X, M, F };
public enum Stat { Con, Str, Agi, Rea, Mnd, Int, Wil, Cha };
public enum UnitType { Normal, Grass };

[System.Serializable]
public class Unit {

  public string name;
  public Gender gender;

  public UnitRace race;
  public List<UnitClass> classes;

  public List<UnitType> type;
  public Dictionary<Stat, int> stats;
  public Dictionary<Stat, int> statsUnique;
  public int level;

  public float move;
  public float jump;
  public int healthCur, healthMax, manaCur, manaMax, speedCur, speedMax, speedTemp;
  public int apCur, apMax;

  public List<Action> actions;
  public List<Passive> passives;

  public string bodyResource;
  public string profileResource;

  public Unit() {
    // Populate unit fields with default values
    name = "Boi " + UnityEngine.Random.Range(1, 100).ToString();
    gender = Gender.N;
    race = new Hume();
    classes = new List<UnitClass>();
    classes.Add(new Dweller());

    statsUnique = new Dictionary<Stat, int>();

    bodyResource = "UnitBody";
    profileResource = "";

    UpdateDerivedStats();
    ResetCurrentStats();
  }

  public void UpdateDerivedStats() {
    race.SetUnit(this);

    foreach (KeyValuePair<Stat, int> stat in statsUnique) {
      stats[stat.Key] += stat.Value;
    }

    level = 0;

    foreach (UnitClass c in classes) {
      level += c.level;
      c.ApplyBonusToUnit(this);
    }

    healthMax = 14 + stats[Stat.Con] * 5 + stats[Stat.Str] + stats[Stat.Agi] / 2;
    manaMax = 5 + stats[Stat.Wil] * 5 + stats[Stat.Mnd] + stats[Stat.Int] / 3;
    speedMax = stats[Stat.Rea] + stats[Stat.Int];

    apMax = 2;
    apCur = apMax;

    actions = new List<Action>();
    actions.Add(new Slap());

    passives = new List<Passive>();
  }

  public void ResetCurrentStats() {
    healthCur = healthMax;
    manaCur = manaMax;
    speedCur = speedMax;
    speedTemp = 0;
  }

  public GameObject GetBody() {
    return (GameObject)Resources.Load(bodyResource, typeof(GameObject));
  }

  public void ApplyDamage(int damage) {
    healthCur -= damage;
    healthCur = Mathf.Max(healthCur, 0);
  }

  public bool IsAlive() {
    return healthCur > 0;
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

    GameObject profileImage = panel.GetChild(2).gameObject;
    GameObject profileImageDefault = panel.GetChild(1).gameObject;

    profileImageDefault.SetActive(profileResource == "");
    profileImage.SetActive(profileResource != "");

    if (profileResource != "") {
      profileImage.GetComponent<Image>().sprite = (Sprite)Resources.Load(profileResource, typeof(Sprite));
    }
  }

  public void SetStatusUI(GameObject statusUI) {
    Transform screen = statusUI.transform.Find("BackImage");
    screen.Find("NameText").gameObject.GetComponent<Text>().text = name;
    screen.Find("ClassText").gameObject.GetComponent<Text>().text = classes[0].ToString();
    screen.Find("GenderText").gameObject.GetComponent<Text>().text = gender.ToString();

    if (profileResource != "")
      screen.Find("ProfileImage").gameObject.GetComponent<Image>().sprite = (Sprite)Resources.Load(profileResource, typeof(Sprite));

    for (int i = 1; i <= 3; i++) {
      if (type.Count < i)
        screen.Find("Type/Type" + i.ToString()).gameObject.GetComponent<Text>().text = "";
      else
        screen.Find("Type/Type" + i.ToString()).gameObject.GetComponent<Text>().text = type[i - 1].ToString().ToUpper();
    }

    screen.Find("Level/LevelText").gameObject.GetComponent<Text>().text = level.ToString();
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