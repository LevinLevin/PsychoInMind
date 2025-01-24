using UnityEngine;

public class ResourceObject : MonoBehaviour, ITakeDamage
{
    private ObjectPooler objectPooler;

    [SerializeField]
    private float health;

    public ItemEnum resourceTag;

    public void TakeDamage(float value)
    {
        health -= value;
        Debug.Log("Life: " + health);
        if (health <= 0)
        {
            Crumble();
        }
    }

    public void Crumble()
    {
        Debug.Log("Destroyed");
        objectPooler.SpawnFromPool(resourceTag, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        health = Random.Range(0.5f, 1.5f);
    }
}