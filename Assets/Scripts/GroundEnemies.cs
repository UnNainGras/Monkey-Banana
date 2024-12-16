using UnityEngine;

public class GroundEnemies : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public float patrolRange = 5f;  // La distance totale de patrouille (divisée par 2 de chaque côté)
    public float patrolSpeed = 2f;  // Vitesse de déplacement
    public float detectionRadius = 10f;  // Rayon de détection du joueur
    public float chaseSpeed = 3f;  // Vitesse de poursuite du joueur

    private Vector3 startPosition;
    private Vector3 leftPatrolPoint;
    private Vector3 rightPatrolPoint;
    private Transform player;
    private bool isChasing = false;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public Color hitColor = Color.gray;

    private bool movingRight = true;  
   
    private Rigidbody2D rb;

   
    private int playerCollisionCount = 0; 
    private float damageInterval = 1f;  
    private float lastDamageTime = 0f;

    public GameObject hitEffect;

    void Start()
    {
        startPosition = transform.position;
        leftPatrolPoint = startPosition - new Vector3(patrolRange / 2f, 0f, 0f);  
        rightPatrolPoint = startPosition + new Vector3(patrolRange / 2f, 0f, 0f); 
        player = GameObject.FindGameObjectWithTag("Player").transform;

        currentHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        rb = GetComponent<Rigidbody2D>();  
    }

    void Update()
    {
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        CheckPlayerDetection();
        UpdateOrientation();

        if (playerCollisionCount > 0 && Time.time - lastDamageTime >= damageInterval)
        {
            HealthManager.instance.HurtPlayer();
            lastDamageTime = Time.time;
        }
    }

    private void Patrol()
    {
        if (movingRight)
        {
            transform.position = Vector3.MoveTowards(transform.position, rightPatrolPoint, patrolSpeed * Time.deltaTime);

            spriteRenderer.flipX = true;

            if (Vector3.Distance(transform.position, rightPatrolPoint) < 0.1f)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, leftPatrolPoint, patrolSpeed * Time.deltaTime);

            spriteRenderer.flipX = false;

            if (Vector3.Distance(transform.position, leftPatrolPoint) < 0.1f)
            {
                movingRight = true;
            }
        }
    }

    private void CheckPlayerDetection()
    {
        if (player && Vector3.Distance(transform.position, player.position) < detectionRadius)
        {
            isChasing = true;
        }
        else if (player && Vector3.Distance(transform.position, player.position) > detectionRadius)
        {
            isChasing = false;
        }
    }

    private void ChasePlayer()
    {
        if (!player) return;

        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);

        if (targetPosition.x < leftPatrolPoint.x)
        {
            targetPosition = leftPatrolPoint;
        }
        else if (targetPosition.x > rightPatrolPoint.x)
        {
            targetPosition = rightPatrolPoint;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, chaseSpeed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
            Die();
        }
        else
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
            StartCoroutine(HitEffect());
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private System.Collections.IEnumerator HitEffect()
    {
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = originalColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerCollisionCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerCollisionCount = Mathf.Max(0, playerCollisionCount - 1);
        }
    }

    private void UpdateOrientation()
    {
        if (player && isChasing)
        {
            float direction = player.position.x - transform.position.x;

            if (direction > 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (direction < 0)
            {
                spriteRenderer.flipX = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 startPosition = transform.position;
        Vector3 leftPatrolPoint = startPosition - new Vector3(patrolRange / 2f, 0f, 0f);
        Vector3 rightPatrolPoint = startPosition + new Vector3(patrolRange / 2f, 0f, 0f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(leftPatrolPoint, rightPatrolPoint);
    }
}
