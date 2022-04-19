using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
  private Vector2 speed = new Vector3(0, 0);
  public Vector2 realSpeed = new Vector2(0, 0);
  private GameController gameController;
  Rigidbody2D rb;


  void Start()
  {
    gameController = GameObject.Find("GameController").GetComponent<GameController>();
    rb = GetComponent<Rigidbody2D>();
    ReCenter();
  }

  void FixedUpdate()
  {
    if (Mathf.Abs(transform.position.x) > 20) Goal(rb.velocity.x > 0 ? 0 : 1);
    if (gameController.isGameOver)
      rb.velocity = Vector2.ClampMagnitude(rb.velocity, gameController.gameSettings.ballInitialSpeed);
    realSpeed = rb.velocity;
  }

  void OnCollisionEnter2D(Collision2D collider)
  {
    if (!gameController.isGameOver)
      collider.gameObject.GetComponent<AudioSource>().Play();

    if (collider.transform.tag == "Paddle")
    {
      float deltaY = transform.position.y - collider.transform.position.y;
      rb.velocity = new Vector2(rb.velocity.x, deltaY * 10f);
      if (Mathf.Abs(rb.velocity.y) > Mathf.Abs(rb.velocity.x))
      {
        if (rb.velocity.y > 0) rb.velocity = new Vector2(rb.velocity.x, Mathf.Abs(rb.velocity.x));
        if (rb.velocity.y < 0) rb.velocity = new Vector2(rb.velocity.x, 0 - Mathf.Abs(rb.velocity.x));
      }
    }
  }

  void OnTriggerEnter2D(Collider2D collider)
  {
    if (collider.transform.tag == "Goal")
    {
      if (gameController.isGameOver)
      {
        rb.velocity = new Vector2(-1 * rb.velocity.x, rb.velocity.y);
      }
      else
      {
        collider.gameObject.GetComponent<AudioSource>().Play();
        Goal(collider.transform.name == "Goal.Right" ? 0 : 1);
      }
    }
  }

  void Goal(int player)
  {
    gameController.Score(player);
  }

  public void ReCenter()
  {
    speed = new Vector2(0, 0);
    rb.velocity = speed;
    transform.position = new Vector2(0, 0);
    StartCoroutine(RestartBall());
  }

  IEnumerator RestartBall()
  {
    yield return new WaitForSeconds(gameController.gameSettings.pauseTimeAfterScore);
    speed = new Vector2(
      gameController.gameSettings.ballInitialSpeed,
      Random.Range(1.0f, -1.0f) * 3f
    );
    if (Random.value > 0.5f) speed.x = -1 * speed.x;
    rb.velocity = speed;
  }
}
