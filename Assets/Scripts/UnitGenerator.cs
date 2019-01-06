using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitGenerator {

  public virtual Unit NewUnit() {
    return new Unit();
  }
}