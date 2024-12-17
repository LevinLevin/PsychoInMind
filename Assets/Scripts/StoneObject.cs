using UnityEngine;

public class StoneObject : MonoBehaviour, ITakeDamage
{
    public float health;
    public void TakeDamage(float value)
    {
        health -= value;
        if(health <= 0)
        {
            Crumble();
        }
    }

    void Crumble()
    {
        Destroy(gameObject);
    }

    void Start()
    {

        health = 1f;
    }
}
