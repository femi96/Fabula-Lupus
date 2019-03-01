using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapMenu : MonoBehaviour {

  // Map controls when battles/cutscenes happen

  public void MapStartTestBattle() {
    SceneManager.LoadScene("TestBattleScene", LoadSceneMode.Single);
  }

  public void MapStartTestCutscene() {
    SceneManager.LoadScene("TestBattleScene", LoadSceneMode.Single);
  }

  public void MapToParty() {
    SceneManager.LoadScene("TestBattleScene", LoadSceneMode.Single);
  }

  public void MapSaveQuit() {
    Party.SaveParty();
    SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
  }
}