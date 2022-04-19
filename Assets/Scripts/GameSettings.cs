using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class GameSettings 
{
  public int maxScore = 10;
  public string title = "PONG";
  public string quit = "escape";
  public string reset = "r";
  public string end = "x";
  public string player1Up = "w";
  public string player1Down = "s";
  public string player2Up = "up";
  public string player2Down = "down";
  public float paddleMoveSpeed = 8f;
  public float ballInitialSpeed = 3f;
  public int pauseTimeAfterScore = 1;
  public int[] gameColor = {7, 111, 36};

  public static GameSettings LoadFromJSON()
  {
    string filePath = Application.streamingAssetsPath + "/settings.json";
    string jsonFile = File.ReadAllText(filePath);
    Debug.Log(jsonFile);
    return JsonUtility.FromJson<GameSettings>(jsonFile);
  }
}
