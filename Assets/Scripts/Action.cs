using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType {Targeted, Fixed}

public abstract class Action {

  public abstract string GetName();

  public abstract ActionType GetActionType();

  public abstract HashSet<TileNode> GetSelectionTiles(BattleUnit user, BattleGrid battle);

  public abstract void ApplyAction(List<TileNode> targets, BattleGrid battle);
}
