//============================================================
// Author: Isaac Shields
// Date  : 10-12-2024
// Desc  : logic for the game over screen
//============================================================
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class endScreen : MonoBehaviour
{
    public TextMeshProUGUI dist;
    public TextMeshProUGUI coins;
    public Button mainMenuBtn;
    public Button quitBtn;
    public int distanceTraveled;
    public int coinsCollected;
    public TextMeshProUGUI score;
    public PlayerController pc;
    public Button playAgainBtn;
    public void onDeath()
    {
        //initial setup and display
        distanceTraveled = pc.distanceTraveled;
        coinsCollected = pc.diamondCount;
        dist.text = "Distance Traveled: " + distanceTraveled.ToString();
        coins.text = "Coins Collected: " + coinsCollected.ToString();
        score.text = "Score: " + (distanceTraveled * coinsCollected).ToString();
        mainMenuBtn.onClick.AddListener(gotToMain);
        quitBtn.onClick.AddListener(quitGame);
        playAgainBtn.onClick.AddListener(restartGame);

    }

    private void gotToMain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("mainMenu");
    }

    private void quitGame()
    {
        Application.Quit();
    }

    private void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
}
