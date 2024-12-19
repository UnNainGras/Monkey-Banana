using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour
{
    public static ExitTrigger instance;

    private bool isUnlocked = false;

    [SerializeField] private Sprite closedDoorSprite;
    [SerializeField] private Sprite openDoorSprite;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Lock();
    }

    private void Lock()
    {
        isUnlocked = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
        spriteRenderer.sprite = closedDoorSprite;
    }

    public void Unlock()
    {
        isUnlocked = true;
        gameObject.GetComponent<Collider2D>().enabled = true;
        spriteRenderer.sprite = openDoorSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && isUnlocked)
        {
            StartCoroutine(LevelExit());
        }
    }

    private IEnumerator LevelExit()
    {
        yield return new WaitForSeconds(0.1f);

        UIManager.instance.fadeToBlack = true;

        yield return new WaitForSeconds(2f);

        string currentSceneName = SceneManager.GetActiveScene().name;
        int currentLevelNumber = int.Parse(currentSceneName.Replace("Level ", ""));

        if (currentLevelNumber == 4)
        {
            SceneManager.LoadScene("Menu"); 
        }
        else
        {
            string nextLevelName = "Level " + (currentLevelNumber + 1).ToString();
            SceneManager.LoadScene(nextLevelName);
        }
    }
}
