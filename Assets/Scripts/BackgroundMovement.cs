using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMovement : MonoBehaviour
{
    private BoxCollider2D backgroundCollider;
    private Vector2 pos;
    private int sceneBuildIndex;

    [SerializeField] private float speed;
    [SerializeField] private float menuSpeed = 1;
    [SerializeField] private float defaultGameSpeed = 5;

    private void Awake()
    {
        sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneBuildIndex == 0)
        {
            speed = menuSpeed;
        }
        else
        {
            speed = defaultGameSpeed;
        }
        backgroundCollider = GetComponent<BoxCollider2D>(); 
        pos = transform.position; //The default position
    }

    void Update()
    {
        if (sceneBuildIndex == 0) //Menu background move (stable speed)
        {
            Move();
        }
        else //Gameplay background move (increasing speed)
        {
            if (GameManager.Instance.IsStart && !GameManager.GameOver)
            {
                speed = GameManager.Instance.Speed;
                Move();
            }
        }
    }

    void Move()
    {
        transform.Translate(Vector2.left * Time.deltaTime * speed);

        //Repeat background
        if (transform.position.x < pos.x - backgroundCollider.size.x / 2)
        {
            transform.position = pos;
        }
    }
}
