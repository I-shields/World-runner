using System;
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
    public Canvas highScoreCanvas;
    private highScoreHelper hsh;
    public List<GameObject> tbs = new List<GameObject>();
    void Start()
    {
        hsh = new highScoreHelper();
        btn_quitGame.onClick.AddListener(quitGame);
        btn_newGame.onClick.AddListener(startGame);
        btn_highScores.onClick.AddListener(loadHighScoreCanvas);
        highScoreCanvas.GetComponentInChildren<Button>().onClick.AddListener(returnToMain);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void quitGame()
    {
        Application.Quit();
    }

    private void startGame()
    {
        SceneManager.LoadScene("mainGame");
    }

    private void loadHighScoreCanvas()
    {
        this.GetComponent<Canvas>().enabled = false;
        highScoreCanvas.enabled = true;
        List<int> highScores = loadHighScores();
        if(highScores.Count > 0)
        {
            for(int i = 0; i < highScores.Count; i++)
            {
                GameObject textBox = new GameObject("DynamicTMPText");
                TextMeshProUGUI textComponent = textBox.AddComponent<TextMeshProUGUI>();
                textBox.transform.SetParent(highScoreCanvas.transform, false);
                textBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 250 - (75 * i));
                textComponent.text = "Distance: " + highScores[i].ToString();
                textComponent.enableWordWrapping = false;
                textComponent.alignment = TextAlignmentOptions.Center;
                tbs.Add(textBox);
            }
        }
        else
        {
            GameObject textBox = new GameObject("DynamicTMPText");
            TextMeshProUGUI textComponent = textBox.AddComponent<TextMeshProUGUI>();
            textBox.transform.SetParent(highScoreCanvas.transform, false);
            textBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 250);
            textComponent.text = "No high scores";
            tbs.Add(textBox);
        }
        highScores.Clear();

    }

    private List<int> loadHighScores()
    {
        List<int> scores = hsh.getScores();
        return scores;
    }

    private void returnToMain()
    {
        for(int i = tbs.Count - 1; i >= 0; i--)
        {
            Destroy(tbs[i]);
            tbs.RemoveAt(i);
        }
        this.GetComponent<Canvas>().enabled = true;
        highScoreCanvas.GetComponent<Canvas>().enabled = false;
    }
}
