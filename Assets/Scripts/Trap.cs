using UnityEngine;

public class Trap : MonoBehaviour
{
    public float knockbackForceHorizontal = 10f;
    public float knockbackForceVertical = 15f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthManager.instance.HurtPlayer();

            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                Vector2 knockbackDirection;

                if (collision.contacts[0].normal.y < -0.5f)
                {
                    knockbackDirection = Vector2.up * knockbackForceVertical;
                }
                else
                {
                    knockbackDirection = (collision.transform.position - transform.position).normalized;
                    knockbackDirection.y = 0;
                    knockbackDirection *= knockbackForceHorizontal;
                }

                collision.gameObject.GetComponent<Player>().ApplyKnockback(knockbackDirection, 0.3f);
            }
        }
    }
}
