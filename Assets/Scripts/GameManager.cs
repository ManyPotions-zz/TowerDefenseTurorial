using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool gameIsOver; //static Variable carry over next scene.

    public GameObject gameOverUI;

    private void Start()
    {
        gameIsOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsOver)
            return;

       // if (Input.GetKeyDown("e")) //un GetKey temporaire pour tester le gameOVer UI
       //{
       //     EndGame();
       // }

        if (PlayerStats.Lives <= 0)
        {
            EndGame();
        }

    }

    void EndGame()
    {
        gameIsOver = true;
       // Debug.Log("Game Over!");

        gameOverUI.SetActive(true);

    }

}
