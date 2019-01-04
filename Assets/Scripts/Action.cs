using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType {Targeted, Fixed}

[System.Serializable]
public abstract class Action {

  public abstract string GetName();

  public abstract ActionType GetActionType();

  public abstract HashSet<TileNode> GetSelectionTiles(BattleUnit user, BattleGrid battle);

  public virtual void ApplyAction(TileNode target, BattleGrid battle) {}

  public override string ToString() {
    return GetName();
  }
}
