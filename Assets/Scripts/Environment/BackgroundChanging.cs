using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundChanging : MonoBehaviour
{
    public List<Sprite> backgrounds;
    public List<GameObject> backgroundParts;
    public List<GameObject> terrains;

    [SerializeField] private bool upcomingBackgroundChange = false;
    public bool UpcomingBackgroundChange { get => upcomingBackgroundChange; set => upcomingBackgroundChange = value; }

    [SerializeField] private float changeInterval = 20; //Change the background every 20 seconds
    [SerializeField] private int currentBackground = 0;
    [SerializeField] private int currentTerrain = 0;
    private Coroutine currentCoroutine = null;
    private float screenSize;
    public float ScreenSize { get => screenSize; }

    private void Awake()
    {
        screenSize = Camera.main.orthographicSize * 2 * Camera.main.aspect;
        Debug.Log(Camera.main.orthographicSize + " " + screenSize);
    }

    void Start()
    {
        //Debug.Log("Start");
        currentCoroutine = StartCoroutine(ChangeBackground());
    }

    IEnumerator ChangeBackground()
    {
        yield return new WaitForSeconds(GameManager.Instance.CountdownTime);
        while (true)
        {
            //Estimated time that the last obstacle of the current
            //background moves from the right bound to the left bound of the screen
            float timeBeforeChange = screenSize / GameManager.Instance.Speed;

            yield return new WaitForSeconds(changeInterval - timeBeforeChange);
            upcomingBackgroundChange = true;
            yield return new WaitForSeconds(timeBeforeChange);

            if (!GameManager.GameOver)
            {
                //Change to the next background in the list
                currentBackground = (currentBackground + 1) % backgrounds.Count;
                SpawnManager.Instance.CurrentSprite = currentBackground;
                SceneTransition.Instance.Transition();
                upcomingBackgroundChange = false;

                //Wait for the transition to complete
                yield return new WaitForSeconds(1);
                foreach (GameObject part in backgroundParts)
                {
                    part.GetComponent<SpriteRenderer>().sprite = backgrounds[currentBackground];
                }

                //Change to the next terrain in the list
                terrains[currentTerrain].SetActive(false);
                terrains[(currentTerrain + 1) % terrains.Count].SetActive(true);
                currentTerrain = (currentTerrain + 1) % terrains.Count;
            }
            else
            {
                StopCoroutine(currentCoroutine);
            }
        }
    }
}
