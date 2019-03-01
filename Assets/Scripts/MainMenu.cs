using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

  public void MainPlay() {
    Party.LoadParty(0);
    SceneManager.LoadScene("MapScene", LoadSceneMode.Single);
  }

  public void MainClear() {
    Party.GenerateParty(0);
    Party.SaveParty();
  }

  public void MainQuit() {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
    Application.OpenURL(webplayerQuitURL);
#else
    Application.Quit();
#endif
  }

  public void MainOpenTwitter() {
    Application.OpenURL("https://twitter.com/imef96");
  }
}
