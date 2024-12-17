using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData _item;

    public const float radius = 1;

    void Update()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, radius, transform.forward, out hit))
        {
            if(hit.collider.TryGetComponent(out ICollectItems collectItems))
            {
                collectItems.CollectItem(_item);
            }
        }
    }

    void OnDrawGizmos()
    {
        // Draw a sphere at the position of the GameObject this script is attached to
        Gizmos.DrawSphere(transform.position, 1f);
        Gizmos.color = Color.red;
    }
}
