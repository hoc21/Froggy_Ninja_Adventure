using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private Animator anim;
    private bool playerIsInFire = false; // Biến theo dõi trạng thái người chơi trong lửa
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerLife playerLife = other.gameObject.GetComponent<PlayerLife>();
            StartCoroutine(DelayFire(2f));
            if (playerIsInFire)
            {
                playerLife.Die();
                CheckAnim();
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsInFire = false;
        }
    }

    IEnumerator DelayFire(float delay)
    {
        yield return new WaitForSeconds(delay);
        CheckAnim();
        playerIsInFire = true;
    }

    void CheckAnim()
    {
        anim.SetBool("fire", true);
    }
}
