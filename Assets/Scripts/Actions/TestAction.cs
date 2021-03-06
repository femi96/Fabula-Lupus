﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestAction : Action {

  public override string GetName() {
    return "TestAction";
  }

  public override ActionType GetActionType() {
    return ActionType.Targeted;
  }

  public override HashSet<TileNode> GetSelectionTiles(BattleUnit user, BattleGrid battle) {
    HashSet<TileNode> set = new HashSet<TileNode>();
    set.Add(battle.GetTile(user.GetPosKey()));
    return set;
  }
}
