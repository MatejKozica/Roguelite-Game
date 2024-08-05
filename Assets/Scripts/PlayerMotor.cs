using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : PlayerController
{
    public int playerSpeed;
    public int playerJumpHeight;
    public Animator anim;
    public SpriteRenderer sprite;

    private bool facingRight = true;

    void Start(){
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update() {
        UpdatePlayer();
    }

    void UpdatePlayer() {
        float pSpeed = Input.GetAxis("Horizontal");

        Vector2 move = Vector2.zero;
        
        if(!crouched && !Input.GetKey(KeyCode.LeftControl)) { 
            move.x = Input.GetAxis("Horizontal");
        } else if(!crouched && Input.GetKey(KeyCode.LeftControl)) { 
            move.x = Input.GetAxis("Horizontal") * 2;
        } 

        if(Input.GetButton("Jump") && grounded) {
            anim.SetTrigger("onJump");
            if(Mathf.Abs(velocity.x) > 0) {
                velocity.y = playerJumpHeight + Mathf.Abs(velocity.x * 0.2f); 
            } else {
                velocity.y = playerJumpHeight;
            }
        } else if(Input.GetButtonDown("Jump")) { 
            if(velocity.y > 0) {
                velocity.y *= 0.5f;
            }
        } else {
            crouched = false;
        }

        if(grounded)
            anim.SetBool("isGrounded", true);
        else
            anim.SetBool("isGrounded", false);
        
        if(move.x == 0) {
            anim.SetBool("isWalking", false);
        } else {
            anim.SetBool("isWalking", true);
        }

        if(move.x < 0)
            facingRight = false;
        else 
            facingRight = true;

        if(move.x < 0) {
            sprite.flipX = true;
        }  else if(move.x > 0) {
            sprite.flipX = false;
        }

        targetVelocity = move * playerSpeed;
    }
}
