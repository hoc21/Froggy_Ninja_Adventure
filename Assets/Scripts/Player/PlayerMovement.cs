using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private BoxCollider2D coll;
    [SerializeField] private LayerMask jumpableGround;
    private float dirX;
    [SerializeField]private float moveSpeed = 7f;
    [SerializeField]private float jumpForce = 7f;
    [SerializeField] private AudioSource jumpSoundEffect;
    public float hurtForce = 10f;
    private int jumpsRemaining = 2; // Số lần nhảy được phép

    private enum MovementState { idle,running,jumpping,falling}
    private MovementState state = MovementState.idle;


    public Enemy enemy1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        //Enemy enemy = other.gameObject.GetComponent<Enemy>();

    }

    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed,rb.velocity.y);
        if (Input.GetButtonDown("Jump") && jumpsRemaining > 0)
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpsRemaining--;
        }  
        UpdateAnimationUpdate(); 
    }
    private void UpdateAnimationUpdate()
    {
        MovementState state;
        if(dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if(dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }
        if(rb.velocity.y> 0.1f)
        {
            state = MovementState.jumpping;
        }
        else if(rb.velocity.y < -0.1f)
        {
            state = MovementState.falling;
        }
        anim.SetInteger("state",(int)state);
        
        if (jumpsRemaining == 0)
        {
            anim.SetBool("doubleJump", true);
        }
        else
        {
            anim.SetBool("doubleJump", false);
        }
    }
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center,coll.bounds.size,0f,Vector2.down,0.1f,jumpableGround);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PowerUp"))
        {
            Destroy(other.gameObject);
            jumpForce = 10f;
            moveSpeed = 10f;
            GetComponent<SpriteRenderer>().color = Color.blue;
            StartCoroutine(ResetPower());
        }
    }

    private IEnumerator ResetPower()
    {
        yield return new WaitForSeconds(10);
        jumpForce = 7f;
        moveSpeed = 7f;
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (IsGrounded())
        {
            jumpsRemaining = 2; // Đặt lại số lần nhảy khi chạm đất
        }
        if(other.gameObject.CompareTag("Enemy"))
        {
            if(state == MovementState.falling)
            {
                DieEnemy(other.gameObject);
            }
            // else
            // {
            //     if (other.gameObject.transform.position.x > transform.position.x)
            //     {
            //         rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
            //     }
            //     else
            //     {
            //         rb.velocity = new Vector2(hurtForce, rb.velocity.y);
            //     }
            // }
        }
    }
    void DieEnemy(GameObject enemy){
        enemy.GetComponent<Enemy>().JumpedOn();
    }
}