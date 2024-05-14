using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public static Leaderboard Instance;
    public DataSaver dataSaver;
    
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

    [System.Serializable]
    public class DataSaver
    {
        public List<HighScoreEntry> highScoreEntries = new List<HighScoreEntry>();
    }
}
