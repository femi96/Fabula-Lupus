using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit {

  public Unit unit;
  public int team;
  private Vector2Int key;
  public GameObject body;
  private Vector2Int faceDirection;

  public BattleUnit(Unit u, Vector2Int k, int t, GameObject g) {
    unit = u;
    key = k;
    team = t;
    body = g;
    faceDirection = new Vector2Int(1, 0);
  }

  public override string ToString() {
    return unit.ToString();
  }

  public Vector2Int GetPosKey() {
    return key;
  }

  public Vector3 GetPos() {
    return body.transform.position;
  }

  public void SetPosKey(Vector2Int k) {
    key = k;
  }

  public void SetFaceDirection(Vector2Int face) {
    faceDirection = face;
  }

  public Quaternion GetFaceRotation() {
    return Quaternion.LookRotation(new Vector3(faceDirection.x, 0, faceDirection.y), Vector3.up);
  }
}
