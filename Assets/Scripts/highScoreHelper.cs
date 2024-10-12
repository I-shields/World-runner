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

    private static string path = Application.dataPath + "/Resources/Highscores.txt";
    private List<int> scores = new List<int>();

    //get high scores and save them to a list
    private void readData()
    {
        if(File.Exists(path))
        {
            using(StreamReader reader = new StreamReader(path))
            {
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    scores.Add(int.Parse(line.Trim()));
                }
            }
        }
        else
        {
            Debug.Log("File not found");
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
        using (StreamWriter writer = new StreamWriter(path, false))
        {
            int i = 0;
            while(i < scores.Count && i < 5)
            {
                writer.WriteLine(scores[i]);
                i++;
            }
        }
    }
}
