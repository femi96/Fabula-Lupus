using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slap : Action {

  public override string GetName() {
    return "Slap";
  }

  public override ActionType GetActionType() {
    return ActionType.Targeted;
  }

  public override HashSet<TileNode> GetSelectionTiles(BattleUnit user, BattleGrid battle) {
    TileNode center = battle.GetTile(user.position);
    return battle.GetRangeTiles(center, 1.25f);
  }

  public override void ApplyAction(List<TileNode> targets, BattleGrid battle) {
    return;
  }
}