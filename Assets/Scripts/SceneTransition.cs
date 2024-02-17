using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;
    public Animator transition;

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

    public void TransitionAndLoadScene()
    {
        GetComponentInChildren<CanvasGroup>().alpha = 1;
        StartCoroutine(StartTransitionAndLoadScene());
    }

    public void Transition()
    {
        StartCoroutine(StartTransition());
    }

    IEnumerator StartTransitionAndLoadScene()
    {
        
        transition.SetBool("Transition", true);
        yield return new WaitForSeconds(1); //Wait for the transition to complete
        if (!GameManager.Instance.IsReplay)
        {
            SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameManager.Instance.IsReplay = false;
        }
        yield return new WaitForSeconds(0.1f);
        transition.SetBool("Transition", false);
    }

    IEnumerator StartTransition()
    {
        transition.SetBool("Transition", true);
        yield return new WaitForSeconds(1); //Wait for the transition to complete
        transition.SetBool("Transition", false);
    }
}
