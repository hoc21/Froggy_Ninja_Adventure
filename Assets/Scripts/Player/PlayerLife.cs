using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    [SerializeField] private AudioSource deadSoundEffect;
    [SerializeField] private HealthBar healthBar;
    private float initialMaxHealth = 1f;
    public float knockbackForce = 5f; // Độ mạnh của lực đẩy khi chạm vào Trap

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Trap"))
        {

            healthBar.Damage(0.1f);
            // Kiểm tra xem player còn máu không
            if (Health.totalHealth > 0f)
            {
                // Nếu còn máu, thực hiện lực đẩy sang hai bên
                Vector2 knockbackDirection = new Vector2(other.contacts[0].point.x > transform.position.x ? -1 : 1, 1).normalized;
                rb.velocity = Vector2.zero; // Đặt vận tốc về 0 trước khi áp dụng lực đẩy
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                anim.SetTrigger("hurt");
                StartCoroutine(ReturnToIdle());
            }
            else
            {    
                Die();
            }
        }
        if(other.gameObject.CompareTag("RestartLevel"))
        {
            Die();
        }
    }
    
    public void Die()
    {
        deadSoundEffect.Play();
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("dead");
    }

    IEnumerator ReturnToIdle()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("idle");
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Health.totalHealth = initialMaxHealth;
        healthBar.SetSize(initialMaxHealth);
    }
}