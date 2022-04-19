using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
  public bool isGameOver = true;
  public AudioClip gameStartSound;
  public AudioClip gameOverSound;
  public GameSettings gameSettings;

  private int[] scores = new int[] { 0, 0 };
  private Text[] UIScores = new Text[2];
  private Text UITitle;
  private GameObject[] paddles = new GameObject[2];
  private GameObject gameOverText;
  private Ball ball;
  private AudioSource audioSource;

  void Awake()
  {
    gameSettings = GameSettings.LoadFromJSON();
  }

  void Start()
  {
    ball = GameObject.Find("Ball").GetComponent<Ball>();
    UIScores[0] = GameObject.Find("Score.Left").GetComponent<Text>();
    UIScores[1] = GameObject.Find("Score.Right").GetComponent<Text>();
    UITitle = GameObject.Find("Title").GetComponent<Text>();
    paddles[0] = GameObject.Find("Paddle.Left");
    paddles[1] = GameObject.Find("Paddle.Right");
    gameOverText = GameObject.Find("Gameover");
    UITitle.text = gameSettings.title;
    ColorObjects();

    paddles[0].SetActive(false);
    paddles[1].SetActive(false);
    gameOverText.SetActive(false);

    audioSource = GetComponent<AudioSource>();
    Cursor.visible = false;

    ResetGame();
  }

  void Update()
  {
    if (Input.GetKey(gameSettings.quit))
    {
      Application.Quit();
    }
    if (Input.GetKeyDown(gameSettings.reset))
    {
      ResetGame();
    }
    if (Input.GetKeyDown(gameSettings.end))
    {
      ball.ReCenter();
      GameOver(false);
    }
  }

  public void Score(int player)
  {
    scores[player]++;
    UIScores[player].text = scores[player].ToString("00");
    ball.ReCenter();

    if (scores[player] >= gameSettings.maxScore) GameOver();
  }

  void GameOver(bool playSound = true)
  {
    gameOverText.SetActive(true);
    foreach (GameObject paddle in paddles)
    {
      paddle.SetActive(false);
    }
    if (playSound)
      audioSource.PlayOneShot(gameOverSound);

    isGameOver = true;
  }

  void ResetGame()
  {
    scores = new int[] { 0, 0 };
    UIScores[0].text = "00";
    UIScores[1].text = "00";

    gameOverText.SetActive(false);

    foreach (GameObject paddle in paddles)
    {
      paddle.SetActive(true);
      paddle.transform.position = new Vector3(paddle.transform.position.x, 0, 0);
    }

    audioSource.PlayOneShot(gameStartSound);

    ball.ReCenter();
    isGameOver = false;
  }

  void ColorObjects()
  {
    Color color = new Color(
      (float)gameSettings.gameColor[0] / 255f,
      (float)gameSettings.gameColor[1] / 255f,
      (float)gameSettings.gameColor[2] / 255f
    );
    Debug.Log(color);
    Object[] allObjects = FindObjectsOfType(typeof(GameObject));
    foreach (GameObject go in allObjects)
      if (go.layer == 6)
      {
        go.GetComponent<Renderer>().material.SetColor("_Color", color);
      }
      else if (go.layer == 5)
      {
        if (go.GetComponent<Text>())
          go.GetComponent<Text>().color = color;
      }
  }

  // No longer used
  private static string GetArg(string name)
  {
    var args = System.Environment.GetCommandLineArgs();
    for (int i = 0; i < args.Length; i++)
    {
      if (args[i] == "-" + name && args.Length > i + 1)
      {
        return args[i + 1];
      }
    }
    return null;
  }
}
