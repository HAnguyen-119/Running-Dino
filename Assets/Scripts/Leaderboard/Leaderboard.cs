using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public static Leaderboard Instance;
    public DataSaver dataSaver;

    private List<HighScoreEntry> highScoreEntries;
    public List<HighScoreEntry> HighScoreEntries { get => highScoreEntries; }
    private readonly int numberOfEntries = 5;
    private readonly int maxNumberOfEntries = 10;

    private void Awake()
    {
        if (Instance != null)
        { 
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        if (!Directory.Exists(Application.persistentDataPath + "/HighScores/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/HighScores/");
        };

        highScoreEntries = LoadData();
        highScoreEntries.Sort((HighScoreEntry x, HighScoreEntry y) => y.Score.CompareTo(x.Score));
    }

    //Save current leaderboard
    public void SaveData(List<HighScoreEntry> list)
    {
        dataSaver.highScoreEntries = list;
        XmlSerializer serializer = new XmlSerializer(typeof(DataSaver));
        FileStream stream = new FileStream(Application.persistentDataPath + "/HighScores/highscores.xml", FileMode.Create);
        serializer.Serialize(stream, dataSaver);
        stream.Close();
    }

    //Load the latest leaderboard
    public List<HighScoreEntry> LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/HighScores/highscores.xml"))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DataSaver));
            FileStream stream = new FileStream(Application.persistentDataPath + "/HighScores/highscores.xml", FileMode.Open);
            dataSaver = serializer.Deserialize(stream) as DataSaver;
        }
        return dataSaver.highScoreEntries;
    }

    public void AddNewEntry(string name, int score)
    {
        highScoreEntries.Add(new HighScoreEntry(name, score));
        highScoreEntries.Sort((HighScoreEntry x, HighScoreEntry y) => y.Score.CompareTo(x.Score));
        if (highScoreEntries.Count > maxNumberOfEntries)
        {
            highScoreEntries.RemoveAt(maxNumberOfEntries);
        }
        //UpdateScoreDisplay();
        SaveData(highScoreEntries);
    }

    public int GetHighestScore()
    {
        if (highScoreEntries.Count == 0) return 0;
        return highScoreEntries[0].Score;
    }

    public int GetLowestScoreOnBoard()
    {
        if (highScoreEntries.Count < numberOfEntries) return 0;
        return highScoreEntries[numberOfEntries - 1].Score;
    }

    [System.Serializable]
    public class DataSaver
    {
        public List<HighScoreEntry> highScoreEntries = new List<HighScoreEntry>();
    }
}
