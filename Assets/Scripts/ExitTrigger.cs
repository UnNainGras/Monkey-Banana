using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour
{
    public static ExitTrigger instance;

    private bool isUnlocked = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Lock();
    }

    private void Lock()
    {
        isUnlocked = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
        Debug.Log("Sortie verrouillée. Collectez toutes les pièces pour déverrouiller !");
    }

    public void Unlock()
    {
        isUnlocked = true;
        gameObject.GetComponent<Collider2D>().enabled = true;
        Debug.Log("Sortie déverrouillée !");
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
        string nextLevelName = "Level " + (currentLevelNumber + 1).ToString();

        SceneManager.LoadScene(nextLevelName);
    }
}
