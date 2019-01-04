using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PartySave {
  // Mutable data type for a party's save data

  public int partyID = 0;
  public Unit[] partyUnits = new Unit[0];
}
