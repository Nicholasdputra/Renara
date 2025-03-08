using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Animator anim;
    private SpriteRenderer sr;
    
    private float speed = 5f;
    private Vector2 input;
    private Vector2 lastMoveDirection;
    private bool facingLeft = true;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        ProcessInput();
        Animate();
        if(input.x < 0 && facingLeft || input.x > 0 && !facingLeft)
        {
            Flip();
        }
        // saving last position
        SaveSystem.currentSave.currentPlayerData.position = transform.position;
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
            Debug.Log(lastMoveDirection);
        }
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input.Normalize();
    }

    private void Animate()
    {
        anim.SetFloat("MoveX", input.x);
        anim.SetFloat("MoveY", input.y);
        anim.SetFloat("MoveMagnitude", input.magnitude);
        anim.SetFloat("LastMoveX", lastMoveDirection.x);
        anim.SetFloat("LastMoveY", lastMoveDirection.y);
    }

    private void Flip()
    {
        sr.flipX = !sr.flipX;
        facingLeft = !facingLeft;
    }
}
