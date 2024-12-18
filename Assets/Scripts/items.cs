using UnityEngine;

public class items : MonoBehaviour
{
    public enum ItemType { Heart, Sword, Feather }
    public ItemType itemType;
    [SerializeField] private GameObject pickupEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ApplyEffect();

            if (pickupEffect != null)
            {
                Instantiate(pickupEffect, transform.position, Quaternion.identity);
            }

            gameObject.SetActive(false);
        }
    }

    private void ApplyEffect()
    {
        switch (itemType)
        {
            case ItemType.Heart:
                RestoreHealth();
                break;

            case ItemType.Sword:
                BoostDamage(15f);
                break;

            case ItemType.Feather:
                BoostSpeed(10f);
                break;
        }
    }

    private void RestoreHealth()
    {
        if (HealthManager.instance.currentHealth < 5) 
        {
            HealthManager.instance.currentHealth++;
            HealthManager.instance.DisplayHearts(); 
            Debug.Log("Un cœur a été ramassé ! Vie restaurée.");
        }
        else
        {
            Debug.Log("Vie déjà au maximum.");
        }
    }

    private void BoostDamage(float duration)
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.BoostDamage(duration);
        }
    }

    private void BoostSpeed(float duration)
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.BoostSpeed(duration);
        }
    }
}
