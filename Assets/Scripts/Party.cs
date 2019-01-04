using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class Party : MonoBehaviour {

  void Start() {
    bool loadSuccess = LoadParty(0);

    if (!loadSuccess) {
      GenerateParty(0);
      SaveParty();
    }
  }

  void Update() {}

  // Party has all units
  private int partyID;
  private List<Unit> partyUnits;

  // Generate Party
  private void GenerateParty(int ID) {
    partyID = 0;
    partyUnits = new List<Unit>();
    partyUnits.Add(new Unit());
  }

  // Get file path for party from its ID
  private string GetFilePath(int ID) {
    return Application.persistentDataPath + "/party_" + ID + ".fls";
  }

  // Load party from file, returns success
  private bool LoadParty(int ID) {
    // 0: Get file path
    string saveFilePath = GetFilePath(ID);

    if (File.Exists(saveFilePath)) {
      try {

        // 1: Load file and close file stream
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(saveFilePath, FileMode.Open);
        PartySave save = (PartySave)bf.Deserialize(file);
        file.Close();

        // 2: Set party from save
        SetPartyFromSave(save);
        return true;
      } catch (System.FormatException e) { Debug.Log(e); }
    }

    return false;
  }

  // Save this party to a file
  private void SaveParty() {
    try {
      // 0: Get file path
      string saveFilePath = GetFilePath(partyID);

      // 1: Create save instance
      PartySave save = CreatePartySave();

      // 2: Save file and close file stream
      BinaryFormatter bf = new BinaryFormatter();
      FileStream file = File.Create(saveFilePath);
      bf.Serialize(file, save);
      Debug.Log("Party_" + partyID + " saved to " + saveFilePath);
      file.Close();

      // 3: Ending save
      // Post save operations if necessary
    } catch (System.FormatException e) { Debug.Log(e); }
  }

  // Create a party save representation of this party
  private PartySave CreatePartySave() {
    PartySave save = new PartySave();
    save.partyID = partyID;
    save.partyUnits = partyUnits.ToArray();
    return save;
  }

  // Sets this party from a party save representation
  public void SetPartyFromSave(PartySave save) {
    partyID = save.partyID;
    partyUnits = new List<Unit>(save.partyUnits);
  }

  // Party controls when battles happen, is persistant

  // Start a battle
  public void StartBattle() {
    // Disable party camera
    // Enable wand and wand camera
    // Load BattleGrid Scene
  }

  // End a battle
  public void EndBattle() {
    // Enable party camera
    // Disable wand and wand camera
    // Unload BattleGrid Scene

    // End of battle changes to macro game
    SaveParty();
  }

  // Party controls world menus
  private void UpdateUI() {

  }
}