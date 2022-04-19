using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    private GameController gameController;
    Rigidbody2D rb;
    private string upkey;
    private string downkey;

    void Start() {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        rb = GetComponent<Rigidbody2D>();
        upkey = gameObject.name == "Paddle.Left" 
          ? gameController.gameSettings.player1Up
          : gameController.gameSettings.player2Up;
        downkey = gameObject.name == "Paddle.Left" 
          ? gameController.gameSettings.player1Down
          : gameController.gameSettings.player2Down;
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(0,0);
        if (Input.GetKey(upkey))
        {
            if (checkCollision(Vector3.up, "Wall")) return;

            transform.Translate(Vector3.up * gameController.gameSettings.paddleMoveSpeed * Time.deltaTime);
            return;
        } else if (Input.GetKey(downkey))
        {
            if (checkCollision(Vector3.down, "Wall")) return;

            transform.Translate(Vector3.down * gameController.gameSettings.paddleMoveSpeed * Time.deltaTime);
            return;
        }

    }

    bool checkCollision(Vector3 direction, string tag)
    {
        float distance = transform.localScale.y * .5f;
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, direction, distance);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if (hit.transform.tag == tag) return true;
        }
        return false;
    }
}
