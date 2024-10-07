using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class highScoreHelper
{
    private static string path = "Assets/Resources/Highscores.txt";
    private List<int> scores = new List<int>();
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

    public List<int> getScores()
    {
        readData();
        return scores;
    }


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
