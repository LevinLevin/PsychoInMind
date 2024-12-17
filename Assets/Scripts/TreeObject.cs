using UnityEngine;

public class TreeObject : MonoBehaviour, ITakeDamage
{
    public float health;

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
        Destroy(gameObject);
    }

    void Start()
    {
        health = 1f;
    }
}
