using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static bool GameOver = false;

    [SerializeField] private float speed = 5;
    [SerializeField] private int score = 0;
    [SerializeField] private int highScore = 0;
    [SerializeField] private bool isStart = false;
    [SerializeField] private bool isPause = false;
    [SerializeField] private bool isReplay = false;
    [SerializeField] private int countdownTime = 3;

    public float Speed { get { return speed; } set { speed = value; } }
    public bool IsStart { get { return isStart; } set { isStart = value; } }
    public bool IsPause { get { return isPause; } set { isPause = value; } }
    public bool IsReplay { get { return isReplay; } set { isReplay = value; } }

    public int CountdownTime { get { return countdownTime; } set { countdownTime = value; } }
    public LeaderboardDisplay leaderboardDisplay;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        highScore = leaderboardDisplay.GetHighestScore();
        StartCoroutine(UpdateScore());
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0 && Input.GetKeyDown(KeyCode.P) && !GameOver && isStart)
        {
            ChangePaused();
        }  
    }

    private void ChangePaused()
    {
        if (!isPause)
        {
            Time.timeScale = 0;
            isPause = true;
            UIHandler.Instance.pausePanel.SetActive(true);
        } 
        else
        {
            Time.timeScale = 1;
            isPause = false;
            UIHandler.Instance.pausePanel.SetActive(false);
        }
    }

    IEnumerator UpdateScore()
    {
        GameOver = false;
        score = 0;
        speed = 5;

        SpawnManager.Instance.MinInterval = 1.2f;
        SpawnManager.Instance.MaxInterval = 2.2f;
        SpawnManager.Instance.CurrentSprite = 0;
        SpawnManager.Instance.UpcomingBackgroundChange = false;
        UIHandler.Instance.gameOverPanel.SetActive(false);

        yield return new WaitForSeconds(1.5f);

        UIHandler.Instance.highScoreText.text = "High score : " + highScore;
        UIHandler.Instance.scoreText.text = "Score : " + score;
        
        yield return new WaitForSeconds(1.5f);

        UIHandler.Instance.countdown.gameObject.SetActive(true);
        int timeLeft = countdownTime;
        while (timeLeft > 0)
        {
            UIHandler.Instance.countdown.text = Convert.ToString(timeLeft);
            yield return new WaitForSeconds(1);
            timeLeft--;
        }

        UIHandler.Instance.countdown.gameObject.SetActive(false);
        UIHandler.Instance.pauseText.gameObject.SetActive(true);

        IsStart = true;
        SpawnManager.Instance.SpawnObstacle();

        while (!GameOver)
        {
            yield return new WaitForSeconds(1 / speed);
            speed += Time.deltaTime;
            score++;
            if (score > highScore)
            {
                highScore = score;
            }
            if (score == leaderboardDisplay.GetHighestScore() + 1)
            {
                StartCoroutine(NewHighScore());
            }
            UIHandler.Instance.highScoreText.text = "High score : " + highScore;
            UIHandler.Instance.scoreText.text = "Score : " + score;
        }

        //Stop spawning obstacles and pause background music if game over
        SpawnManager.Instance.StopSpawning();
        SoundManager.Instance.AudioSource.Stop();
        UIHandler.Instance.pauseText.gameObject.SetActive(false);
        IsStart = false;

        //Update the top 5 scores on leaderboard
        if (score >= leaderboardDisplay.GetLowestScoreOnBoard())
        {
            leaderboardDisplay.AddNewEntry(UIHandler.Instance.playerName.text, score);
            leaderboardDisplay.Save();
        }
    }

    IEnumerator NewHighScore()
    {
        int time = 5;
        while (time > 0 && !GameOver)
        {
            UIHandler.Instance.newHighScoreText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            UIHandler.Instance.newHighScoreText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            time--;
        }
    }
}
