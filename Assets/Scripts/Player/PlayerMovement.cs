using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerLife player;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private BoxCollider2D coll;
    [SerializeField] private LayerMask jumpableGround;
    private float dirX;
    [SerializeField]private float moveSpeed = 7f;
    [SerializeField]private float jumpForce = 7f;
    [SerializeField] private AudioSource jumpSoundEffect;
    private int jumpsRemaining = 2; // Số lần nhảy được phép
    public bool isJumping = false;
    [SerializeField] private HealthBar healthBar;
    public float knockbackForce = 5f;
    private enum MovementState { idle,running,jumpping,falling}
    private MovementState state = MovementState.idle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        player = GetComponent<PlayerLife>();
    }

    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed,rb.velocity.y);
        if (Input.GetButtonDown("Jump") && jumpsRemaining > 0 )
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpsRemaining--;
            isJumping = true;
        }else if(jumpsRemaining == 2){
            isJumping = false;
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
            if(isJumping)
            {
                DieEnemy(other.gameObject);
            }
            else
            {
                anim.SetTrigger("hurt");
                HandleHealth();
                Vector2 knockbackDirection = new Vector2(other.contacts[0].point.x > transform.position.x ? -1 : 1, 1).normalized;
                rb.velocity = Vector2.zero;
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
    void DieEnemy(GameObject enemy){
        enemy.GetComponent<Enemy>().JumpedOn();
    }
    private void HandleHealth()
    {
        healthBar.Damage(0.1f);
        if (Health.totalHealth > 0f)
        {
            StartCoroutine(ReturnToIdle());
        }
            else
            {    
                player.Die();
            }
    }
    IEnumerator ReturnToIdle()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("idle");
    }
}