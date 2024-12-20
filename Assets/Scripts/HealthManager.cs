using System.Collections;
using TMPro;
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
    [SerializeField] private TextMeshProUGUI invincibilityText;

    private GameObject Player;
    private AudioSource audioSource;
    public bool isInvincible = false;
    private Coroutine invincibilityCoroutine;


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
    if (isInvincible || currentHealth <= 0) return; // Ignorer si invincible

    currentHealth--; 
    DisplayHearts();

    if (currentHealth == 0)
    {
        GameManager.instance.Death(); 
    }

    Instantiate(damageEffect, Player.transform.position, Quaternion.identity);
    audioSource.PlayOneShot(hurtSFX, 2.0f);
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
   public void ActivateInvincibility(float duration)
{
    if (invincibilityCoroutine != null)
    {
        StopCoroutine(invincibilityCoroutine);
    }
    invincibilityCoroutine = StartCoroutine(InvincibilityCoroutine(duration));
}


private IEnumerator InvincibilityCoroutine(float duration)
{
    isInvincible = true;
    invincibilityText.gameObject.SetActive(true); 

    float remainingTime = duration;
    while (remainingTime > 0)
    {
        invincibilityText.text = $"Invincibilité : {remainingTime:F1} s";
        yield return new WaitForSeconds(0.1f); 
        remainingTime -= 0.1f;
    }

    isInvincible = false;
    invincibilityText.gameObject.SetActive(false);
}


}
