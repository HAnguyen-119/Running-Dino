using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    private Coroutine currentCoroutine = null;
    [SerializeField] private GameObject obstacle;
    [SerializeField] private List<Sprite> obstacleSprites;
    private ObjectPool<GameObject> obstaclePool;
    public ObjectPool<GameObject> Pool { get => obstaclePool; }
    private int currentSpriteIndex = 0;
    public int CurrentSpriteIndex { get => currentSpriteIndex; set => currentSpriteIndex = value; }


    [SerializeField] private float spawnInterval = 2.0f;
    public float SpawnInterval { get => spawnInterval; set => spawnInterval = value; } 
    [SerializeField] private float minInterval = 1.2f;
    public float MinInterval { get => minInterval; set => minInterval = value; } 
    [SerializeField] private float maxInterval = 2.2f;
    public float MaxInterval { get => maxInterval; set => maxInterval = value; } 

    [SerializeField] private Vector2 spawnPosition = new Vector2(10, 0);
    [SerializeField] private bool poolCollectionCheck = true;
    [SerializeField] private int poolDefaultCapacity = 10;
    [SerializeField] private int poolMaxSize = 20;

    [SerializeField] private BackgroundChanging backgroundChanging;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
  
        //DontDestroyOnLoad(gameObject);

        obstaclePool = new ObjectPool<GameObject>(CreateObstacle, 
            OnGetFromPool, OnReleaseToPool, OnDestroyObstacle, poolCollectionCheck, poolDefaultCapacity, poolMaxSize);
    }

    //Create a new obstacle
    private GameObject CreateObstacle()
    {
        return Instantiate(obstacle, spawnPosition, obstacle.transform.rotation, transform);
    }

    private void OnGetFromPool(GameObject obstacleInstance)
    {
        obstacleInstance.SetActive(true);
        obstacleInstance.transform.position = spawnPosition;
        obstacleInstance.GetComponent<SpriteRenderer>().sprite = obstacleSprites[currentSpriteIndex];
    }

    private void OnReleaseToPool(GameObject obstacleInstance)
    {
        obstacleInstance.SetActive(false);
    }

    private void OnDestroyObstacle(GameObject obstacleInstance)
    {
        Destroy(obstacleInstance);
    }

    public void SpawnObstacle()
    {
        currentCoroutine = StartCoroutine(StartSpawning());
    }
    
    IEnumerator StartSpawning()
    {
        while (!GameManager.GameOver)
        {
            if (!backgroundChanging.UpcomingBackgroundChange)
            {
                //Decrease the obstacle spawn interval over time
                spawnInterval = Random.Range(minInterval, maxInterval);
                if (minInterval > 0.7) minInterval -= Time.deltaTime;
                if (maxInterval > minInterval) maxInterval -= Time.deltaTime;
            }
            else
            {
                //No obstacles will be spawned while the background is changing
                spawnInterval = backgroundChanging.ScreenSize / GameManager.Instance.Speed + 3;
            }
            //Get an obstacle from pool
            obstaclePool.Get();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void StopSpawning()
    {
        StopCoroutine(currentCoroutine);
    }
}
