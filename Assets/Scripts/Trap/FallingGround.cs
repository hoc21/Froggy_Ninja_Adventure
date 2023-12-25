using System.Collections;
using UnityEngine;

public class FallingGround : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Vector2 initialPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        initialPosition = transform.position;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Delay());
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        rb.bodyType = RigidbodyType2D.Dynamic;

        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }

        yield return new WaitForSeconds(2f);

        // Xuất hiện lại vị trí ban đầu
        rb.bodyType = RigidbodyType2D.Static;
        transform.position = initialPosition;

        if (boxCollider != null)
        {
            boxCollider.enabled = true;
        }
    }
}
