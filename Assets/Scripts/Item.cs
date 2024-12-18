using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData _item;

    public const float radius = 1;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ICollectItems collectItems))
        {
            collectItems.CollectItem(_item);
            Debug.Log("Collected");
            Destroy(gameObject);
        }
    }
}
