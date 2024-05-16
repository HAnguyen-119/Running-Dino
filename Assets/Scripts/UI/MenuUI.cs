using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public static MenuUI Instance;

    //[SerializeField] private LeaderboardDisplay leaderboard;
    //public LeaderboardDisplay Leaderboard { get => leaderboard; }
    [SerializeField] private GameObject customizationPanel;
    public GameObject CustomizationPanel { get => customizationPanel; }
    [SerializeField] private TextMeshProUGUI playerName;
    public TextMeshProUGUI PlayerName { get => playerName; }

    [SerializeField] private Image characterImage;
    [SerializeField] private Sprite[] characterImages;
    [SerializeField] private int currentImageIndex = 0;
    public int CurrentImageIndex { get => currentImageIndex; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        SoundManager.Instance.PlayMenuMusic();
        //leaderboard.Load();
    }

    public void ChoosePreviousImage()
    {
        currentImageIndex = (currentImageIndex - 1 + characterImages.Length) % characterImages.Length;
        characterImage.sprite = characterImages[currentImageIndex];
    }

    public void ChooseNextImage()
    {
        currentImageIndex = (currentImageIndex + 1 + characterImages.Length) % characterImages.Length;
        characterImage.sprite = characterImages[currentImageIndex];
    }

    public void Play()
    {
        SceneTransition.Instance.TransitionAndLoadScene(1);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }
}
