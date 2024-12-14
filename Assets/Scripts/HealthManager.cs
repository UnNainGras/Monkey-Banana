using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance;
    public GameObject damageEffect;

    private int MaxHealth = 5; // 5 full hearts
    public int currentHealth;

    [SerializeField] private Image[] hearts; // Assign these via Inspector (5 heart images)
    [SerializeField] private Sprite FullHeartSprite;
    [SerializeField] private Sprite EmptyHeartSprite;

    private GameObject Player;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Player = GameObject.FindObjectOfType<PlayerController>().gameObject;
        currentHealth = MaxHealth;
        DisplayHearts();
    }

    public void HurtPlayer()
    {
        if (currentHealth > 0)
        {
            currentHealth--; // Reduce health by 1 heart
            DisplayHearts();

            if (currentHealth == 0)
            {
                GameManager.instance.Death(); // Trigger death logic
            }

            Instantiate(damageEffect, Player.transform.position, Quaternion.identity);
        }
    }

    public void DisplayHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = FullHeartSprite; // Full heart
            }
            else
            {
                hearts[i].sprite = EmptyHeartSprite; // Empty heart
            }
        }
    }
}
