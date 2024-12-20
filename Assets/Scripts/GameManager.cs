using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI")]
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text levelText; 

    [SerializeField] private Player player;
    [SerializeField] private AudioClip coinSFX;

    private int coinCount = 0;
    private int totalCoins = 0;
    private bool isGameOver = false;
    private Vector3 playerPosition;
    private string currentLevelName;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        LevelTimer.instance.StartLevelTimer();
        audioSource = GetComponent<AudioSource>();
        currentLevelName = SceneManager.GetActiveScene().name;
        DisplayLevelName();

        playerPosition = player.transform.position;

        FindTotalCoins();
        UpdateGUI();

        UIManager.instance.fadeFromBlack = true;
    }

    public void IncrementCoinCount()
    {
        audioSource.PlayOneShot(coinSFX, 5.0f);
        coinCount++;
        UpdateGUI();

        if (coinCount >= totalCoins)
        {
            UnlockExit();
        }
    }

    private void UpdateGUI()
    {
        coinText.text = $"{coinCount} / {totalCoins}";
    }

    public void Death()
    {
        if (!isGameOver)
        {
            player.DisableControls();
            player.TriggerDeathAnimation();
            isGameOver = true;

            StartCoroutine(DeathCoroutine());
        }
    }

    private void FindTotalCoins()
    {
        pickup[] pickups = GameObject.FindObjectsOfType<pickup>();

        foreach (pickup pickupObject in pickups)
        {
            if (pickupObject.pt == pickup.pickupType.coin)
            {
                totalCoins++;
            }
        }

        if (totalCoins == 0)
        {
            totalCoins = 1;
        }
    }

    private void UnlockExit()
    {
        Debug.Log("Toutes les pieces collectees ! La sortie est maintenant accessible.");
        ExitTrigger.instance.Unlock();
    }

    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(1f);

        UIManager.instance.fadeToBlack = true;

        player.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);

        if (isGameOver)
        {
            SceneManager.LoadScene(currentLevelName);
        }
    }

    private void DisplayLevelName()
    {
        if (levelText != null)
        {
            
            levelText.text = currentLevelName;
        }
    }
}
