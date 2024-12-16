using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Chest : MonoBehaviour
{
    [Header("Items à faire spawn")]
    [SerializeField] private GameObject[] itemPrefabs;
    [SerializeField] private Transform spawnPoint;

    [Header("Sprites d'Ouverture")]
    [SerializeField] private Sprite[] chestSprites;
    private SpriteRenderer spriteRenderer;

    private bool isOpened = false;

    [Header("Rayon de détection du joueur")]
    [SerializeField] private float detectionRadius = 3.5f;

    [Header("Mouvement des items")]
    [SerializeField] private float itemMoveDuration = 1f;

    [Header("Texte d'information")]
    [SerializeField] private TextMeshProUGUI infoText;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = chestSprites[0];

        if (infoText != null)
        {
            infoText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (PlayerIsNearby() && !isOpened)
        {
            if (infoText != null && !infoText.gameObject.activeSelf)
            {
                infoText.gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(OpenChest());
            }
        }
        else
        {
            if (infoText != null && infoText.gameObject.activeSelf)
            {
                infoText.gameObject.SetActive(false);
            }
        }
    }

    private bool PlayerIsNearby()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator OpenChest()
    {
        isOpened = true;

        if (infoText != null)
        {
            infoText.gameObject.SetActive(false);
        }

        for (int i = 1; i < chestSprites.Length; i++)
        {
            spriteRenderer.sprite = chestSprites[i];
            yield return new WaitForSeconds(0.2f);
        }

        int randomIndex = Random.Range(0, itemPrefabs.Length);
        GameObject item = Instantiate(itemPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);

        Vector3 startPos = spawnPoint.position + Vector3.up * 1f;
        Vector3 endPos = spawnPoint.position;
        float elapsedTime = 0;

        while (elapsedTime < itemMoveDuration)
        {
            item.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / itemMoveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        item.transform.position = endPos;

        Debug.Log("Coffre ouvert et item spawn !");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
