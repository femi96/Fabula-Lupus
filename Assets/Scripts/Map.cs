using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMenu : MonoBehaviour {

  // Map controls when battles/cutscenes happen

  // Start a battle
  private void StartBattle() {
    // Disable party camera
    // Enable wand and wand camera
    // Load BattleGrid Scene
  }

  // End a battle
  private void EndBattle() {
    // Enable party camera
    // Disable wand and wand camera
    // Unload BattleGrid Scene

    // End of battle changes to macro game
    // SaveParty();
  }

  public void StartTestBattle() {

  }
}