using UnityEngine;

public class DinoMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2;
    [SerializeField] private Vector2 startPos;
    [SerializeField] private float rightBound = 25;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * speed);
        if (transform.position.x > rightBound)
        {
            transform.position = startPos;
        }
    }
}
