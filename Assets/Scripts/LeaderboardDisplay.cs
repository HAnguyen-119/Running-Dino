using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardDisplay : MonoBehaviour
{
    public List<TextMeshProUGUI> playerNames;
    public List<TextMeshProUGUI> playerScores;
    private readonly int numberOfEntry = 5;
    private List<HighScoreEntry> highScoreEntries;

    private void Awake()
    { 
        //Initial leaderboard
        if (highScoreEntries.Count < numberOfEntry)
        {
            AddNewEntry("Han", 2650);
            AddNewEntry("Adu vjp", 1474);
            AddNewEntry("Abc", 1225);
            AddNewEntry("Han", 934);
            AddNewEntry("wasd", 537);
        }
        UpdateScoreDisplay();
    }

    public void Save()
    {
        Leaderboard.Instance.SaveData(highScoreEntries);
    }

    public void Load()
    {
        highScoreEntries = Leaderboard.Instance.LoadData(); 
    }

    public void UpdateScoreDisplay()
    {
        highScoreEntries.Sort((HighScoreEntry x, HighScoreEntry y) => y.Score.CompareTo(x.Score));
        for (int i = 0; i < numberOfEntry; i++)
        {
            if (i < highScoreEntries.Count)
            {
                playerNames[i].text = highScoreEntries[i].Name;
                playerScores[i].text = Convert.ToString(highScoreEntries[i].Score);
            }
            else
            {
                playerNames[i].text = "N/A";
                playerScores[i].text = "N/A";
            }
        }
    }

    public void AddNewEntry(string name, int score)
    {
        highScoreEntries.Add(new HighScoreEntry(name, score));
        UpdateScoreDisplay();
        Leaderboard.Instance.SaveData(highScoreEntries);
    }

    public int GetHighestScore()
    {
        if (highScoreEntries.Count == 0) return 0;
        return highScoreEntries[0].Score;
    }

    public int GetLowestScoreOnBoard()
    {
        if (highScoreEntries.Count < numberOfEntry) return 0;
        return highScoreEntries[numberOfEntry - 1].Score;
    }
}
