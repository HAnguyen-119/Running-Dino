using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static bool GameOver = false;

    [SerializeField] private int score = 0;
    [SerializeField] private int highScore = 0;

    [SerializeField] private float speed = 5;
    public float Speed { get => speed; set => speed = value; }

    [SerializeField] private bool isStart = false;
    public bool IsStart { get => isStart; set => isStart = value; }
    [SerializeField] private bool isPause = false;
    public bool IsPause { get => isPause; set => isPause = value; }

    [SerializeField] private int countdownTime = 3;
    public int CountdownTime { get => countdownTime; set => countdownTime = value; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        //Leaderboard.Instance.Load();
        highScore = Leaderboard.Instance.GetHighestScore();
        StartCoroutine(UpdateScore());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !GameOver && isStart)
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
            GameplayUI.Instance.PausePanel.SetActive(true);
        } 
        else
        {
            Time.timeScale = 1;
            isPause = false;
            GameplayUI.Instance.PausePanel.SetActive(false);
        }
    }

    IEnumerator UpdateScore()
    {
        GameOver = false;
        score = 0;
        speed = 5;

        SpawnManager.Instance.MinInterval = 1.2f;
        SpawnManager.Instance.MaxInterval = 2.2f;
        SpawnManager.Instance.CurrentSpriteIndex = 0;

        //Wait for the transition to complete
        yield return new WaitForSeconds(1.5f);

        GameplayUI.Instance.UpdateHighScore(highScore);
        GameplayUI.Instance.UpdateScore(score);
        
        yield return new WaitForSeconds(1);

        GameplayUI.Instance.CountdownText.gameObject.SetActive(true);
        int timeLeft = countdownTime;
        while (timeLeft > 0)
        {
            GameplayUI.Instance.CountdownText.text = timeLeft.ToString();
            yield return new WaitForSeconds(1);
            timeLeft--;
        }

        GameplayUI.Instance.CountdownText.gameObject.SetActive(false);
        GameplayUI.Instance.PauseText.gameObject.SetActive(true);
        GameplayUI.Instance.HighScoreText.gameObject.SetActive(true);
        GameplayUI.Instance.ScoreText.gameObject.SetActive(true);

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
            if (score == Leaderboard.Instance.GetHighestScore() + 1)
            {
                GameplayUI.Instance.OnNewHighScore();
            }
            GameplayUI.Instance.UpdateHighScore(highScore);
            GameplayUI.Instance.UpdateScore(score);
        }

        //Stop spawning obstacles and pause background music if game over
        //SpawnManager.Instance.StopSpawning();
        SoundManager.Instance.AudioSource.Stop();
        GameplayUI.Instance.PauseText.gameObject.SetActive(false);
        IsStart = false;

        //Update the top 5 scores on leaderboard
        if (score >= Leaderboard.Instance.GetLowestScoreOnBoard())
        {
            Leaderboard.Instance.AddNewEntry(MenuUI.Instance.PlayerName.text, score);
        }
    }
}
