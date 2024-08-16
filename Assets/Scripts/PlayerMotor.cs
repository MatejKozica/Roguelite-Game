using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : PlayerController
{
    public int playerSpeed;
    public int playerJumpHeight;
    public int playerClimbingSpeed = 8;
    public Animator anim;
    public SpriteRenderer sprite;
    public LayerMask exitDoorMask;
    public LayerMask ladderMask;

    private bool isClimbing;
    private bool isLadderCollision;

    void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        UpdatePlayer();
    }

    void UpdatePlayer()
    {
        Vector2 move = Vector2.zero;
        Collider2D ladderCheck = Physics2D.OverlapCircle(transform.position, 0.1f, ladderMask);

        move.x = Input.GetAxis("Horizontal");

        if (!ladderCheck)
            Physics2D.gravity = new Vector2(0, -9.8f);

        if (Input.GetButton("Jump") && grounded)
        {
            anim.SetTrigger("onJump");
            if (Mathf.Abs(velocity.x) > 0)
            {
                velocity.y = playerJumpHeight + Mathf.Abs(velocity.x * 0.2f);
            }
            else
            {
                velocity.y = playerJumpHeight;
            }
        }
        else if (Input.GetButton("Jump") && ladderCheck != null)
        {
            anim.SetTrigger("onJump");
            if (Mathf.Abs(velocity.x) > 0)
            {
                velocity.y = playerJumpHeight / 2 + Mathf.Abs(velocity.x * 0.2f);
            }
        }
        else if (Input.GetButtonDown("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y *= 0.5f;
            }
        }

        if (ladderCheck != null && Mathf.Abs(Input.GetAxis("Vertical")) > 0f)
        {
            Physics2D.gravity = Vector2.zero;
            velocity.y = playerClimbingSpeed * Input.GetAxis("Vertical");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Is ladder: " + Physics2D.OverlapCircle(transform.position, 0.1f, ladderMask));
            Debug.Log("Is climbing: " + isLadderCollision);
            Debug.Log("Vertical speed: " + Input.GetAxis("Vertical"));
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Collider2D doorCheck = Physics2D.OverlapCircle(transform.position, 0.1f, exitDoorMask);
            if (doorCheck != null)
            {
                Generation.NextLevel();
                doorCheck.GetComponent<ExitDoor>().NextLevel();
            }
        }

        // Animation control
        if (grounded)
            anim.SetBool("isGrounded", true);
        else
            anim.SetBool("isGrounded", false);

        if (move.x == 0)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }

        // Sprite orientation
        if (move.x < 0)
        {
            sprite.flipX = true;
        }
        else if (move.x > 0)
        {
            sprite.flipX = false;
        }

        targetVelocity = move * playerSpeed;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerMotor player = collider.gameObject.GetComponent<PlayerMotor>();
        Treasure treasure = collider.gameObject.GetComponent<Treasure>();
        if (player)
            return;
        else if (treasure)
        {
            treasure.OnCollide(true);
        }
        else if (collider.gameObject.GetComponent<EnemyBase>())
        {
            TakeDamage(collider.GetComponent<EnemyBase>().damage); //collider.transform.position
        }
    }
    public void OnDeath()
    {
        Debug.Log("The player has died");
    }

    public void TakeDamage(int amount)
    {
        GetComponent<LivingBase>().TakeDamage(amount);

    }

    public void TakeDamage(int amount, Vector2 targetPos)
    {
        TakeDamage(amount);
        velocity += new Vector2(targetPos.x >= transform.position.x ? -10 : 10, targetPos.y >= transform.position.y ? -10 : 10);
    }
}
