using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public static class Party {

  // Persistent

  private static void Start() {
    // bool loadSuccess = LoadParty(0);
    bool loadSuccess = false;

    if (!loadSuccess) {
      GenerateParty(0);
      SaveParty();
    }
  }

  // Party has all units
  private static int partyID;
  private static List<Unit> partyUnits;

  // Generate Party
  public static void GenerateParty(int ID) {
    partyID = ID;
    partyUnits = new List<Unit>();
    partyUnits.Add(Keaton.NewUnit());
  }

  // Get file path for party from its ID
  private static string GetFilePath(int ID) {
    return Application.persistentDataPath + "/party_" + ID + ".fls";
  }

  // Load party from file, returns success
  public static bool LoadParty(int ID) {
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
  public static void SaveParty() {
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
  private static PartySave CreatePartySave() {
    PartySave save = new PartySave();
    save.partyID = partyID;
    save.partyUnits = partyUnits.ToArray();
    return save;
  }

  // Sets this party from a party save representation
  private static void SetPartyFromSave(PartySave save) {
    partyID = save.partyID;
    partyUnits = new List<Unit>(save.partyUnits);
  }
}