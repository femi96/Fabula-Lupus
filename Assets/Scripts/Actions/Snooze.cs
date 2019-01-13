using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Snooze : Action {

  public override string GetName() {
    return "Snooze";
  }

  public override ActionType GetActionType() {
    return ActionType.Fixed;
  }

  public override HashSet<TileNode> GetSelectionTiles(BattleUnit user, BattleGrid battle) {
    TileNode center = battle.GetTile(user.GetPosKey());
    return new HashSet<TileNode>() { center };
  }

  public override void ApplyAction(TileNode target, BattleGrid battle) {
    BattleUnit bu = battle.GetUnit(target.GetPos());

    if (bu != null)
      bu.unit.ApplyHeal(30);
  }
}