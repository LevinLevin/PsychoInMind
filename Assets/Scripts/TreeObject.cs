using UnityEngine;

public class TreeObject : MonoBehaviour, ITakeDamage
{
    private ObjectPooler objectPooler;

    public float health;

    public ItemData itemToDrop;

    public void TakeDamage(float value)
    {
        health -= value;
        Debug.Log("Tree Life: " +  health);
        if(health <= 0)
        {
            Fall();
        }
    }

    public void Fall()
    {
        Debug.Log("Tree destroyed");
        objectPooler.SpawnFromPool("Stick", transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    void Start()
    {
        objectPooler = ObjectPooler.Instance;

        health = 1f;
    }
}
