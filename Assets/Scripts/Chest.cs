using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Items à faire spawn")]
    [SerializeField] private GameObject[] itemPrefabs; 
    [SerializeField] private Transform spawnPoint;

    [Header("Animation")]
    private Animator animator; 

    private bool isOpened = false; 

    void Start()
    {
        animator = GetComponent<Animator>(); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && PlayerIsNearby() && !isOpened)
        {
            OpenChest();
        }
    }

    private bool PlayerIsNearby()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    private void OpenChest()
    {
        isOpened = true;

        animator.SetTrigger("Open");

        int randomIndex = Random.Range(0, itemPrefabs.Length);
        Instantiate(itemPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);

        Debug.Log("Coffre ouvert et item spawn !");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
}
