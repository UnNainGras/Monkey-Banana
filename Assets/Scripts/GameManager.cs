using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private TMP_Text coinText;

    [SerializeField] private Player player;

    private int coinCount = 0;
    private int totalCoins = 0;
    private bool isGameOver = false;
    private Vector3 playerPosition;

    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        playerPosition = player.transform.position;

        FindTotalCoins();

        UpdateGUI();

        UIManager.instance.fadeFromBlack = true;
    }

    public void IncrementCoinCount()
    {
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
        Debug.Log("Toutes les pièces collectées ! La sortie est maintenant accessible.");
        ExitTrigger.instance.Unlock();
    }

    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(1f);

        UIManager.instance.fadeToBlack = true;

        player.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);

        player.transform.position = playerPosition;

        if (isGameOver)
        {
            SceneManager.LoadScene(1);
        }
    }
}
