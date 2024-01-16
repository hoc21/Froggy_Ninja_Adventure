using System.Collections;
using UnityEngine;

public class Fan : MonoBehaviour
{
    public float upwardForce = 10f;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ApplyUpwardForce(other.gameObject.GetComponent<Rigidbody2D>());
            anim.SetBool("Fan", true);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            anim.SetBool("Fan", false);
        }
    }

    void ApplyUpwardForce(Rigidbody2D playerRb)
    {
        Vector2 upwardVector = new Vector2(0f, upwardForce);
        playerRb.velocity = Vector2.zero;
        playerRb.AddForce(upwardVector, ForceMode2D.Impulse);
    }
}
