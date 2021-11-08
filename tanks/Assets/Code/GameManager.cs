using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager Singleton;
    public TMP_Text gameOverDisplay;
    public bool gameOver;
    
    public static void setGameOver()
    {
        Singleton.gameOverDisplay.enabled = true;
        Singleton.gameOver = true;
    }
    /// <summary>
    /// displays game over text and limits functionality
    /// </summary>
    public static void setGameStart()
    {
        Singleton.gameOverDisplay.enabled = false;
        Singleton.gameOver = false;
    }

    /// <summary>
    /// checks if game is over
    /// </summary>
    /// <returns>true if game is over</returns>
    public static bool isGameOver()
    {
        return Singleton.gameOver;
    }

    // Start is called before the first frame update
    void Start()
    {
        Singleton = this;
        setGameStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver() && Input.GetKeyUp("r"))
        {
            setGameStart();
            ScoreKeeper.ResetPoints();
            Player.ResetLives();
        }
    }
}
