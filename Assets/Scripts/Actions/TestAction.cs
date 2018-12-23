using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAction : Action {

  public override string GetName() {
    return "TestAction";
  }

  public override ActionType GetActionType() {
    return ActionType.Fixed;
  }

  public override HashSet<TileNode> GetSelectionTiles(BattleUnit user) {
    HashSet<TileNode> set = new HashSet<TileNode>();
    return set;
  }

  public override void ApplyAction(List<TileNode> targets) {
    return;
  }
}
