using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundChanging : MonoBehaviour
{
    private Coroutine currentCoroutine = null;

    [SerializeField] private List<Sprite> backgrounds;
    [SerializeField] private List<GameObject> backgroundParts;
    [SerializeField] List<GameObject> terrains;

    [SerializeField] private bool upcomingBackgroundChange = false;
    public bool UpcomingBackgroundChange { get => upcomingBackgroundChange; set => upcomingBackgroundChange = value; }

    [SerializeField] private float changeInterval = 20; 
    [SerializeField] private int currentBackgroundIndex = 0;
    [SerializeField] private int currentTerrainIndex = 0;
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
                currentBackgroundIndex = (currentBackgroundIndex + 1) % backgrounds.Count;
                SpawnManager.Instance.CurrentSpriteIndex = currentBackgroundIndex;
                SceneTransition.Instance.Transition();
                upcomingBackgroundChange = false;

                //Wait for the transition to complete
                yield return new WaitForSeconds(1);
                foreach (GameObject part in backgroundParts)
                {
                    part.GetComponent<SpriteRenderer>().sprite = backgrounds[currentBackgroundIndex];
                }

                //Change to the next terrain in the list
                terrains[currentTerrainIndex].SetActive(false);
                terrains[(currentTerrainIndex + 1) % terrains.Count].SetActive(true);
                currentTerrainIndex = (currentTerrainIndex + 1) % terrains.Count;
            }
            else
            {
                StopCoroutine(currentCoroutine);
            }
        }
    }
}
