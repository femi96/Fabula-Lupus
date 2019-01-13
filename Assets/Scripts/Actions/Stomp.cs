using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stomp : Action {

  public override string GetName() {
    return "Stomp";
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
      bu.unit.ApplyDamage(40);
  }
}