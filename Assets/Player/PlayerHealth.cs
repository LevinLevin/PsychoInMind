using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider HealthBarSlider;
    public float maxHealth;
    public float curHealth;

    private void Start()
    {
        curHealth = maxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(0.1f);
        }
    }

    public void TakeDamage(float damage)
    {
        if (curHealth >= 0.00f)
        {
            curHealth -= damage;
            HealthBarSlider.value = curHealth;
            Debug.Log("Damage Taken");
        }
    }
    public void Heal(float heal)
    {
        if (curHealth < 1f)
        {
            curHealth += heal;
            HealthBarSlider.value = curHealth;
            Debug.Log("geheilt +1");
        }
    }
}
