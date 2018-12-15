using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MarkerType { Spawn };

public class BattleGridMarker : MonoBehaviour {
  public MarkerType type;
  public int val;
}
