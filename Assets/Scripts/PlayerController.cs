using System.Collections;
using System.Collections.Generic;
//#if UNITY_EDITOR
//using UnityEditor.Animations;
//#endif
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private Animator playerAnimator;

    public List<Sprite> dinoImages;
    public List<RuntimeAnimatorController> animatorControllers;

    [SerializeField] private float jumpForce = 5;
    [SerializeField] private bool isOnGround = true;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        //Set the player sprite and animator controller based on the dino that player chose in Menu
        GetComponent<SpriteRenderer>().sprite = dinoImages[UIHandler.Instance.CurrentImage];
        playerAnimator.runtimeAnimatorController = animatorControllers[UIHandler.Instance.CurrentImage];
    }

    void Update()
    {
        if (GameManager.Instance.IsStart)
        {
            playerAnimator.SetFloat("Speed", GameManager.Instance.Speed);
        }

        //Jump
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isOnGround && !GameManager.GameOver)
        {
            SoundManager.Instance.PlayJumpSoundEffect();
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isOnGround = false;
        } 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        } 

        //Collide with an obstacle
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            SoundManager.Instance.PlayDeathSoundEffect();
            GameManager.GameOver = true;
            playerAnimator.SetBool("Death", true);
            UIHandler.Instance.gameOverPanel.SetActive(true);
        }
    }
}
