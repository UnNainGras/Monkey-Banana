using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;        
    public float lifetime = 5f;    
    private Vector3 direction;     

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HealthManager.instance.HurtPlayer();

            Destroy(gameObject);
        }

        if (other.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
