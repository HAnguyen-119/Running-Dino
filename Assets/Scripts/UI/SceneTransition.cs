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

    IEnumerator StartTransition()
    {
        transition.SetBool("Transition", true);
        yield return new WaitForSeconds(1); //Wait for the transition to complete
        transition.SetBool("Transition", false);
    }

    public void Transition()
    {
        StartCoroutine(StartTransition());
    }

    IEnumerator StartTransitionAndLoadScene(int sceneIndex)
    {
        //Time.timeScale = 1;
        transition.SetBool("Transition", true);
        yield return new WaitForSecondsRealtime(1); //Wait for the transition to complete
        SceneManager.LoadScene(sceneIndex);
        Time.timeScale = 1;
        yield return new WaitForSeconds(0.1f);
        transition.SetBool("Transition", false);
    }

    public void TransitionAndLoadScene(int sceneIndex)
    {
        GetComponentInChildren<CanvasGroup>().alpha = 1;
        StartCoroutine(StartTransitionAndLoadScene(sceneIndex));
    }
}
