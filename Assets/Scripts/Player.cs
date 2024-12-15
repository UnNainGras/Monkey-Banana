using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] float m_doubleJumpForce = 6.0f;
    [SerializeField] float m_rollForce = 6.0f;
    [SerializeField] bool m_noBlood = false;
    [SerializeField] GameObject m_slideDust;
    [SerializeField] float m_attackRange = 1.0f;  // Portée de l'attaque
    [SerializeField] int m_attackDamage = 1;     // Dégâts infligés par l'attaque

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_Player m_groundSensor;
    private bool m_grounded = false;
    private bool m_rolling = false;
    private bool m_canDoubleJump = false;
    private int m_facingDirection = 1;
    private int m_currentAttack = 0;
    private float m_timeSinceAttack = 0.0f;
    private float m_delayToIdle = 0.0f;
    private float m_rollDuration = 8.0f / 14.0f;
    private float m_rollCurrentTime;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Player>();
    }

    void Update()
    {
        // Augmenter le timer pour le combo d'attaque
        m_timeSinceAttack += Time.deltaTime;

        // Timer pour la durée du roulé
        if (m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        if (m_rollCurrentTime > m_rollDuration)
            m_rolling = false;

        // Vérifier si le joueur vient de toucher le sol
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
            m_canDoubleJump = true;
        }

        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Gérer l'entrée et les déplacements --
        float inputX = Input.GetAxis("Horizontal");

        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        if (!m_rolling)
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Gérer les animations --
        if (Input.GetKeyDown("q") && !m_rolling)
            m_animator.SetTrigger("Hurt");

        // Attaque
        else if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling)
        {
            m_currentAttack++;

            if (m_currentAttack > 3)
                m_currentAttack = 1;

            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            m_animator.SetTrigger("Attack" + m_currentAttack);
            m_timeSinceAttack = 0.0f;

            PerformAttack();
        }

        // Block
        else if (Input.GetMouseButtonDown(1) && !m_rolling)
        {
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
        }

        else if (Input.GetMouseButtonUp(1))
            m_animator.SetBool("IdleBlock", false);

        // Roll
        else if (Input.GetKeyDown("left shift") && !m_rolling)
        {
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
        }

        // Jump
        else if (Input.GetKeyDown("space") && (m_grounded || m_canDoubleJump) && !m_rolling)
        {
            m_animator.SetTrigger("Jump");

            if (!m_grounded && m_canDoubleJump)
            {
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, 0);
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_doubleJumpForce);
                m_canDoubleJump = false;
            }
            else if (m_grounded)
            {
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                m_groundSensor.Disable(0.2f);
            }
        }

        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }
        else
        {
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }
    }

    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = transform.position;
        else
            spawnPosition = transform.position;

        if (m_slideDust != null)
        {
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("killzone"))
        {
            GameManager.instance.Death();
        }
    }

    public void TriggerDeathAnimation()
    {
        m_animator.SetBool("noBlood", m_noBlood);
        m_animator.SetTrigger("Death");
    }

    public void DisableControls()
    {
        m_speed = 0;
        m_jumpForce = 0;
        m_rollForce = 0;
    }

    private void PerformAttack()
    {
        float attackOffsetX = m_facingDirection * m_attackRange; 
        Vector3 attackPosition = transform.position + new Vector3(attackOffsetX, 1, 0); 

        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(attackPosition, m_attackRange);

        foreach (var enemy in enemiesInRange)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<Enemies>().TakeDamage(m_attackDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        float offsetX = m_facingDirection * m_attackRange;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(offsetX, 1, 0), m_attackRange);
    }
}
