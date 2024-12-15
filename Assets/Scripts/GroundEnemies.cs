using UnityEngine;

public class GroundEnemies : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public float patrolRange = 5f;  // La distance totale de patrouille (divis�e par 2 de chaque c�t�)
    public float patrolSpeed = 2f;  // Vitesse de d�placement
    public float detectionRadius = 10f;  // Rayon de d�tection du joueur
    public float chaseSpeed = 3f;  // Vitesse de poursuite du joueur

    private Vector3 startPosition;
    private Vector3 leftPatrolPoint;
    private Vector3 rightPatrolPoint;
    private Transform player;
    private bool isChasing = false;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public Color hitColor = Color.gray;

    private bool movingRight = true;  // D�termine si l'ennemi se d�place vers la droite

    // Pour emp�cher le slime de traverser le sol
    private Rigidbody2D rb;

    // Gestion des d�g�ts inflig�s au joueur
    private bool isTouchingPlayer = false;
    private float damageInterval = 0.5f;  // Intervalle entre les d�g�ts r�p�t�s
    private float lastDamageTime = 0f;  // Temps du dernier d�g�t inflig�

    void Start()
    {
        startPosition = transform.position;
        leftPatrolPoint = startPosition - new Vector3(patrolRange / 2f, 0f, 0f);  // Point de patrouille gauche
        rightPatrolPoint = startPosition + new Vector3(patrolRange / 2f, 0f, 0f); // Point de patrouille droite
        player = GameObject.FindGameObjectWithTag("Player").transform;

        currentHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        rb = GetComponent<Rigidbody2D>();  // R�cup�rer le Rigidbody2D pour g�rer les collisions
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

        // V�rifier si des d�g�ts doivent �tre inflig�s au joueur
        if (isTouchingPlayer && Time.time - lastDamageTime >= damageInterval)
        {
            HealthManager.instance.HurtPlayer();
            lastDamageTime = Time.time;  // Mettre � jour le temps du dernier d�g�t
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
            Die();
        }
        else
        {
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
            HealthManager.instance.HurtPlayer();
            lastDamageTime = Time.time;  
            isTouchingPlayer = true;  
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTouchingPlayer = false;
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
