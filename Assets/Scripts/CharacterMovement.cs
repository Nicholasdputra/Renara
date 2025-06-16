using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Animator anim;
    private SpriteRenderer sr;
    
    public float speed = 9f;
    private Vector2 input;
    private Vector2 lastMoveDirection;
    private bool facingLeft = true;

    public bool canMove;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        canMove = true;
        if (SceneManager.GetActiveScene().name == "Overworld")
        {
            Debug.Log("Loading player position from save data");
            Debug.Log("Current Player Position: " + SaveSystem.currentSave.currentPlayerData.position);
            transform.position = SaveSystem.currentSave.currentPlayerData.position;
        }
    }

    private void Update()
    {
        ProcessInput();

        if (canMove)
        {
            speed = 9f;
        }
        else speed = 0f;

        Animate();

        if (input.x < 0 && facingLeft || input.x > 0 && !facingLeft)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        rb2d.velocity = input * speed;
    }

    private void ProcessInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if ((moveX == 0 && moveY == 0) && (input.x != 0 || input.y != 0))
        {
            lastMoveDirection = input;
        }
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input.Normalize();
    }

    private void Animate()
    {
        if(canMove)
        {
            anim.SetFloat("MoveX", input.x);
            anim.SetFloat("MoveY", input.y);
            anim.SetFloat("MoveMagnitude", input.magnitude);
            anim.SetFloat("LastMoveX", lastMoveDirection.x);
            anim.SetFloat("LastMoveY", lastMoveDirection.y);
        }
        else
        {
            anim.SetFloat("MoveMagnitude", 0);
        }
        
    }

    private void Flip()
    {
        sr.flipX = !sr.flipX;
        facingLeft = !facingLeft;
    }


}
