using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float leftBound = -20;

    private SpawnManager spawnManager;

    private void Awake()
    {
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();    
    }

    void Update()
    {
        if (!GameManager.GameOver)
        {
            speed = GameManager.Instance.Speed;
            transform.Translate(Vector2.left * Time.deltaTime * speed);

            //Rlease obstacle to pool if it's out of bound
            if (transform.position.x < leftBound || !GameManager.Instance.IsStart)
            {
                SpawnManager.Instance.GetPool().Release(gameObject);
            }
        }

        //Release obstacle to pool if the active scene is Menu
        if (SceneManager.GetActiveScene().buildIndex == 0 && gameObject.activeSelf)
        {
            SpawnManager.Instance.GetPool().Release(gameObject);
        }
    }
}
