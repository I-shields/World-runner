//============================================================
// Author: Isaac Shields
// Date  : 10-12-2024
// Desc  : handles high score
//============================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class highScoreHelper
{
    private List<int> scores = new List<int>();

    //get high scores and save them to a list
    private void readData()
    {
        for (int i = 0; i < 5; i++)
        {
            scores.Add(PlayerPrefs.GetInt(i.ToString(), 0));
        }
    }

    //public function to get the high scores
    public List<int> getScores()
    {
        readData();
        return scores;
    }


    //saves the high scores and adds new ones if applicable
    public void saveScores(int score)
    {
        readData();
        scores.Add(score);
        scores.Sort((a, b) => b.CompareTo(a));
        int i = 0;
        while(i < 5 && i < scores.Count)
        {
            PlayerPrefs.SetInt(i.ToString(), scores[i]);
            i++;
        }
    }
}
