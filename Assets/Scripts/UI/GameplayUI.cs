using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    public static GameplayUI Instance;

    [SerializeField] private TextMeshProUGUI scoreText;
    public TextMeshProUGUI ScoreText { get => scoreText; }
    [SerializeField] private TextMeshProUGUI highScoreText;
    public TextMeshProUGUI HighScoreText { get => highScoreText; }
    [SerializeField] private TextMeshProUGUI newHighScoreText;
    [SerializeField] private TextMeshProUGUI pauseText;
    public TextMeshProUGUI PauseText { get => pauseText; }

    [SerializeField] private GameObject pausePanel;
    public GameObject PausePanel { get => pausePanel; }
    [SerializeField] private Button pauseMenuButton;
    [SerializeField] private TextMeshProUGUI countdownText;
    public TextMeshProUGUI CountdownText { get => countdownText; set => countdownText = value; }

    [SerializeField] private Button soundButton;
    [SerializeField] private Slider volumeChanger;
    [SerializeField] private Sprite muteImage;
    [SerializeField] private Sprite unmuteImage;
    [SerializeField] private bool isMute = false;

    [SerializeField] private GameObject gameOverPanel;
    public GameObject GameOverPanel { get => gameOverPanel; }
    [SerializeField] private Button replayButton;
    [SerializeField] private Button gameOverMenuButton;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        SoundManager.Instance.PlayGameMusic();
        volumeChanger.onValueChanged.AddListener(ChangeVolume);
        pauseMenuButton.onClick.AddListener(() => SceneTransition.Instance.TransitionAndLoadScene(0));
        gameOverMenuButton.onClick.AddListener(() => SceneTransition.Instance.TransitionAndLoadScene(0));
        replayButton.onClick.AddListener(() => SceneTransition.Instance.TransitionAndLoadScene(1));
    }

    void Update()
    {
        if (volumeChanger.value == 0)
        {
            soundButton.gameObject.GetComponent<Image>().sprite = muteImage;
            isMute = true;
        }
        else
        {
            soundButton.gameObject.GetComponent<Image>().sprite = unmuteImage;
            isMute = false;
        }
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void UpdateHighScore(int highScore)
    {
        highScoreText.text = "High score: " + highScore.ToString();
    }

    public void OnNewHighScore()
    {
        StartCoroutine(NewHighScore());
    }

    IEnumerator NewHighScore()
    {
        int time = 5;
        while (time > 0 && !GameManager.GameOver)
        {
            newHighScoreText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            newHighScoreText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            time--;
        }
    }

    void ChangeVolume(float volume)
    {
        SoundManager.Instance.AudioSource.volume = volume;
    }

    public void ChangeMute()
    {
        if (isMute)
        {
            soundButton.gameObject.GetComponent<Image>().sprite = unmuteImage;
            isMute = false;
        }
        else
        {
            soundButton.gameObject.GetComponent<Image>().sprite = muteImage;
            isMute = true;
        }
    }

    /*
    public void Replay()
    {
        SceneTransition.Instance.TransitionAndLoadScene(1);
    }

    public void BackToMenu()
    {
        SceneTransition.Instance.TransitionAndLoadScene(0);
    }
    */

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }
}
