using System.Collections.Generic;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance;

    public TextMeshProUGUI gameName;
    public GameObject playButton;
    public GameObject rankingButton;
    public GameObject exitButton;
    public Sprite buttonUpImage;
    public Sprite buttonDownImage;

    public GameObject customPanel;
    public TextMeshProUGUI playerName;

    public TextMeshProUGUI pauseText;
    public GameObject pausePanel;
    public GameObject soundButton;
    public Sprite unmuteImage;
    public Sprite muteImage;
    public Slider changeVolume;
    [SerializeField] private bool isMute = false;
 
    public GameObject gameOverPanel;
    public TextMeshProUGUI countdown;
    public LeaderboardDisplay leaderboardDisplay;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI newHighScoreText;

    public Image dinoImage;
    public List<Sprite> dinoImages;
    [SerializeField] private int currentImage = 0;
    public int CurrentImage { get { return currentImage; } } 
 
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    
        highScoreText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        pauseText.gameObject.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        GameManager.Instance.gameObject.SetActive(false);
        SpawnManager.Instance.gameObject.SetActive(false);
        leaderboardDisplay.Load();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            highScoreText.gameObject.SetActive(true);
            scoreText.gameObject.SetActive(true);
            gameName.gameObject.SetActive(false);
            playButton.SetActive(false);
            rankingButton.SetActive(false);
            exitButton.SetActive(false);
            customPanel.SetActive(false);

            if (changeVolume.value == 0)
            {
                soundButton.GetComponent<Image>().sprite = muteImage;
                isMute = true; 
            } 
            else
            {
                soundButton.GetComponent<Image>().sprite = unmuteImage;
                isMute = false;
            }
        }
        else
        {
            gameName.gameObject.SetActive(true);
            playButton.SetActive(true);
            rankingButton.SetActive(true);
            exitButton.SetActive(true);
            highScoreText.gameObject.SetActive(false);
            scoreText.gameObject.SetActive(false);
            newHighScoreText.gameObject.SetActive(false);
            pauseText.gameObject.SetActive(false);
            pausePanel.SetActive(false);
        }
    }

    public void Play()
    {
        GameManager.Instance.gameObject.SetActive(true);
        SpawnManager.Instance.gameObject.SetActive(true);
        GameManager.Instance.IsPause = false;
        gameOverPanel.SetActive(false);
        SoundManager.Instance.PlayGameMusic();
        GameManager.Instance.StartGame();
    }

    public void ExitGame()
    {
        leaderboardDisplay.Save();
#if UNITY_EDITOR 
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }

    public void BackToMenu()
    {
        GameManager.Instance.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);
        GameManager.Instance.IsStart = false;
        GameManager.GameOver = true;
        SoundManager.Instance.PlayMenuMusic(); 
        Time.timeScale = 1;
    }

    public void ChooseLastImage()
    {
        currentImage = (currentImage - 1 + dinoImages.Count) % dinoImages.Count;
        dinoImage.sprite = dinoImages[currentImage];
    }

    public void ChooseNextImage()
    {
        currentImage = (currentImage + 1) % dinoImages.Count;
        dinoImage.sprite = dinoImages[currentImage];
    }

    public void Mute()
    {
        if (!isMute)
        {
            soundButton.GetComponent<Image>().sprite = muteImage;
            changeVolume.value = 0;
            isMute = true;
        }
        else
        {
            soundButton.GetComponent<Image>().sprite = unmuteImage;
            changeVolume.value = 1;
            isMute = false;
        }
    }
}
