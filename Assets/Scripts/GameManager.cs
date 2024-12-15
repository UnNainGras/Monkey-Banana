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
    private int gemCount = 0;
    private bool isGameOver = false;
    private Vector3 playerPosition;

    [SerializeField] GameObject levelCompletePanel;
    [SerializeField] TMP_Text leveCompletePanelTitle;
    [SerializeField] TMP_Text levelCompleteCoins;

    private int totalCoins = 0;
  
    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        UpdateGUI();
        UIManager.instance.fadeFromBlack = true;
        playerPosition = player.transform.position;

        FindTotalPickups();
    }

    public void IncrementCoinCount()
    {
        coinCount++;
        UpdateGUI();
    }
    public void IncrementGemCount()
    {
        gemCount++;
        UpdateGUI();
    }

    private void UpdateGUI()
    {
        coinText.text = coinCount.ToString();
  
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


    public void FindTotalPickups()
    {

        pickup[] pickups = GameObject.FindObjectsOfType<pickup>();

        foreach (pickup pickupObject in pickups)
        {
            if (pickupObject.pt == pickup.pickupType.coin)
            {
                totalCoins += 1;
            }
           
        }


      
    }
    public void LevelComplete()
    {
       


        levelCompletePanel.SetActive(true);
        leveCompletePanelTitle.text = "LEVEL COMPLETE";



        levelCompleteCoins.text = "COINS COLLECTED: "+ coinCount.ToString() +" / " + totalCoins.ToString();
 
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
