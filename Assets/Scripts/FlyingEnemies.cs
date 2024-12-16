using UnityEngine;

public class FlyingEnemies : MonoBehaviour
{
    // Nouveau : Points de vie
    public int maxHealth = 3;
    private int currentHealth;

    // Nouveau : Hit effect
    public Color hitColor = Color.gray;
    private Color originalColor;
    private bool isHit = false;

    private SpriteRenderer spriteRenderer;

    public enum EnemyType { Shooter, Kamikaze }
    public EnemyType enemyType;

    public float patrolRadius = 5f;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 3f;
    private Vector3 startPosition;
    private Vector3 patrolTarget;

    public float detectionRadius = 10f;
    public float attackRange = 8f;
    public float stopChaseRange = 5f;
    private Transform player;

    public GameObject projectilePrefab;
    public float shootCooldown = 2f;
    private float shootTimer = 0f;
    public bool predictiveShooting = false;

    public float explosionRange = 1f;

    public GameObject hitEffect;

    private enum State { Patrolling, Chasing, Stopped, Attacking }
    private State currentState;

    void Start()
    {
        startPosition = transform.position;
        patrolTarget = GetRandomPatrolPoint();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = State.Patrolling;

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        // Initialisation des points de vie
        currentHealth = maxHealth;

        chaseSpeed += Random.Range(-0.5f, 0.5f);
        shootCooldown += Random.Range(-0.3f, 0.3f);
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
                CheckPlayerDetection();
                break;
            case State.Chasing:
                ChasePlayer();
                break;
            case State.Stopped:
                StopAndShoot();
                break;
            case State.Attacking:
                AttackPlayer();
                break;
        }

        UpdateOrientation();
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
        if (isHit) yield break;

        isHit = true;
        spriteRenderer.color = hitColor; 
        yield return new WaitForSeconds(0.2f); 
        spriteRenderer.color = originalColor; 
        isHit = false;
    }

    void Patrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, patrolTarget, patrolSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, patrolTarget) < 0.1f)
        {
            patrolTarget = GetRandomPatrolPoint();
        }
    }

    Vector3 GetRandomPatrolPoint()
    {
        Vector2 randomPoint = Random.insideUnitCircle * patrolRadius;
        return startPosition + new Vector3(randomPoint.x, randomPoint.y, 0f);
    }

    void CheckPlayerDetection()
    {
        if (player && Vector3.Distance(transform.position, player.position) < detectionRadius)
        {
            currentState = State.Chasing;
        }
    }

    void ChasePlayer()
    {
        if (!player) return;

        transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, player.position) <= stopChaseRange)
        {
            currentState = State.Stopped;
        }

        if (enemyType == EnemyType.Shooter && Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            currentState = State.Attacking;
        }

        if (Vector3.Distance(transform.position, player.position) > detectionRadius)
        {
            currentState = State.Patrolling;
        }
    }

    void StopAndShoot()
    {
        if (!player) return;

        if (enemyType == EnemyType.Shooter)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                ShootProjectile();
                shootTimer = shootCooldown;
            }
        }

        if (Vector3.Distance(transform.position, player.position) > stopChaseRange)
        {
            currentState = State.Chasing;
        }
    }

    void AttackPlayer()
    {
        if (enemyType != EnemyType.Shooter || !player) return;

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            ShootProjectile();
            shootTimer = shootCooldown;
        }

        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            currentState = State.Stopped;
        }
    }

    void ShootProjectile()
    {
        if (!projectilePrefab) return;

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb)
        {
            Vector3 direction;

            if (predictiveShooting && player.TryGetComponent<Rigidbody2D>(out Rigidbody2D playerRb))
            {
                Vector3 playerVelocity = playerRb.velocity;
                direction = ((player.position + playerVelocity * 0.5f) - transform.position).normalized;
            }
            else
            {
                direction = (player.position - transform.position).normalized;
            }

            projectile.GetComponent<Bullet>().SetDirection(direction);
            rb.velocity = direction * 5f;
        }
    }

    private void UpdateOrientation()
    {
        if (player)
        {
            float direction = player.position.x - transform.position.x;

            if (direction > 0)
            {
                spriteRenderer.flipX = false; // Regarde à droite
            }
            else if (direction < 0)
            {
                spriteRenderer.flipX = true; // Regarde à gauche
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPosition, patrolRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        if (enemyType == EnemyType.Shooter)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, stopChaseRange);
        }
        if (enemyType == EnemyType.Kamikaze)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, explosionRange);
        }
    }
}
