using UnityEngine;

public class ResourceItem : MonoBehaviour
{
    private PlayerInventory playerInventory;

    public ItemEnum RescourceName;

    public void Start()
    {
        playerInventory = PlayerInventory.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInventory.CollectItem(RescourceName);
            gameObject.SetActive(false);
        }
    }
}
