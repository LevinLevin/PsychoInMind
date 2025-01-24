using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    Image HealthBarSlider;

    [Range(0f, 1f)]
    public float maxHealth, curHealth;

    #region Singleton
    public static PlayerHealth Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    private void Start()
    {
        HealthBarSlider.fillAmount = curHealth;
    }

    /// <summary>
    /// Updates the health value of the player and sets the UI
    /// </summary>
    /// <param name="heal">how much should be add or be taken away</param>
    public void UpdateLifeValue(float heal)
    {
        curHealth = Mathf.Clamp01(curHealth + heal);

        HealthBarSlider.fillAmount = curHealth;
    }
}
