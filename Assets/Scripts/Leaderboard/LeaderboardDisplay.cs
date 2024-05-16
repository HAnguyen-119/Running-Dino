using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardDisplay : MonoBehaviour
{
    public List<TextMeshProUGUI> playerNames;
    public List<TextMeshProUGUI> playerScores;
    private readonly int numberOfEntry = 5;

    private void Start()
    {
        UpdateScoreDisplay();
    }

    public void UpdateScoreDisplay()
    {
        for (int i = 0; i < numberOfEntry; i++)
        {
            if (i < Leaderboard.Instance.HighScoreEntries.Count)
            {
                playerNames[i].text = Leaderboard.Instance.HighScoreEntries[i].Name;
                playerScores[i].text = Convert.ToString(Leaderboard.Instance.HighScoreEntries[i].Score);
            }
            else
            {
                playerNames[i].text = "N/A";
                playerScores[i].text = "N/A";
            }
        }
    }  
}
