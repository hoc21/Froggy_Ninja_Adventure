using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private PlayerMovement playerMovement;
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
        playerMovement = GetComponent<PlayerMovement>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Trap"))
        {
            healthBar.Damage(0.1f);
            if (Health.totalHealth > 0f)
            {
                Vector2 knockbackDirection = new Vector2(other.contacts[0].point.x > transform.position.x ? -1 : 1, 1).normalized;
                rb.velocity = Vector2.zero; // Đặt vận tốc về 0 trước khi áp dụng lực đẩy
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                anim.SetTrigger("hurt");
                anim.SetTrigger("idle");
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

public void RestartLevel()
{
    Debug.Log("Restarting level...");

    // In giá trị hiện tại của respawnPoint
    Debug.Log("Current respawn point: " + playerMovement.respawnPoint);

    // Cập nhật vị trí checkpoint trước khi tải lại cảnh
    transform.position = playerMovement.respawnPoint;

    // In giá trị mới của vị trí người chơi
    Debug.Log("New position: " + transform.position);

    // Tải lại cảnh
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    // Reset thông tin khác cần thiết (ví dụ: điều trị máu)
    Health.totalHealth = initialMaxHealth;
    healthBar.SetSize(initialMaxHealth);

    Debug.Log("Level restarted!");
}

}
