using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainMenu : MonoBehaviour
{
    public Button btn_newGame;
    public Button btn_highScores;
    public Button btn_quitGame;
    void Start()
    {
        btn_quitGame.onClick.AddListener(quitGame);
        btn_newGame.onClick.AddListener(startGame);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void quitGame()
    {
        if(UnityEditor.EditorApplication.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        Application.Quit();
    }

    private void startGame()
    {
        SceneManager.LoadScene("mainGame");
    }
}
