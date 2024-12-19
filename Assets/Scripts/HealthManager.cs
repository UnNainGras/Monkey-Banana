using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance;
    public GameObject damageEffect;

    private int MaxHealth = 5; 
    public int currentHealth;

    [SerializeField] private Image[] hearts; 
    [SerializeField] private Sprite FullHeartSprite;
    [SerializeField] private Sprite EmptyHeartSprite;
    [SerializeField] private AudioClip hurtSFX;

    private GameObject Player;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Player = GameObject.FindObjectOfType<Player>().gameObject;
        currentHealth = MaxHealth;
        DisplayHearts();
    }

    public void HurtPlayer()
    {
        if (currentHealth > 0)
        {
            currentHealth--; 
            DisplayHearts();

            if (currentHealth == 0)
            {
                GameManager.instance.Death(); 
            }

            Instantiate(damageEffect, Player.transform.position, Quaternion.identity);

            audioSource.PlayOneShot(hurtSFX, 2.0f);
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
