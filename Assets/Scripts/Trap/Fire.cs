using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour 
{
    private Animator anim;
    bool isPlayer = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Lấy tham chiếu đến script PlayerLife từ đối tượng Player
            PlayerLife playerLife = other.gameObject.GetComponent<PlayerLife>();

            // Kiểm tra nếu tham chiếu không null trước khi gọi hàm Die
            if (playerLife != null)
            {
                StartCoroutine(DelayFire(1f, playerLife));
            }
        }
    }
 
    IEnumerator DelayFire(float delay, PlayerLife playerLife)
    {
        yield return new WaitForSeconds(delay);

        // isPlayer = true;
        // playerLife.Die();
        // CheckAnim();

         if (isPlayer)
        {
            playerLife.Die();
        }

        isPlayer = !isPlayer;
        CheckAnim();

        yield return new WaitForSeconds(delay);
        isPlayer = false;
        CheckAnim();
    }

    void CheckAnim()
    {
        anim.SetBool("fire", isPlayer);
    }
}
