using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Slap : Action {

  public override string GetName() {
    return "Slap";
  }

  public override ActionType GetActionType() {
    return ActionType.Targeted;
  }

  public override HashSet<TileNode> GetSelectionTiles(BattleUnit user, BattleGrid battle) {
    TileNode center = battle.GetTile(user.GetPosKey());
    return battle.GetRangeTiles(center, 1.0f);
  }

  public override void ApplyAction(TileNode target, BattleGrid battle) {
    BattleUnit bu = battle.GetUnit(target.GetPos());

    if (bu != null)
      bu.unit.ApplyDamage(100);
  }
}