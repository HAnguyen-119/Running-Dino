using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float leftBound = -20;
    void Update()
    {
        if (!GameManager.GameOver)
        {
            speed = GameManager.Instance.Speed;
            transform.Translate(Vector2.left * Time.deltaTime * speed);

            //Rlease obstacle to pool if it's out of bound
            if (transform.position.x < leftBound || !GameManager.Instance.IsStart)
            {
                SpawnManager.Instance.Pool.Release(gameObject);
            }
        }
    }
}
