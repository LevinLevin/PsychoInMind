using UnityEngine;
using UnityEngine.UI;

public class HungerThirstManager : MonoBehaviour
{
    [SerializeField]
    Image hungerBar, thirstBar;

    [Range(0f, 1f)]
    public float hungerValue, thirstValue;

    [SerializeField] private float hungerDrainRate = 0.01f;
    [SerializeField] private float thirstDrainRate = 0.02f;

    #region Singelton
    public static HungerThirstManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    private void Start()
    {
        hungerBar.fillAmount = hungerValue;
        thirstBar.fillAmount = thirstValue;
    }

    private void Update()
    {
        DecreaseHungerAndThirst(Time.deltaTime);
    }

    private void DecreaseHungerAndThirst(float deltaTime)
    {
        // Decrease hunger and thirst based on their respective rates.
        hungerValue = Mathf.Clamp01(hungerValue - hungerDrainRate * deltaTime);
        thirstValue = Mathf.Clamp01(thirstValue - thirstDrainRate * deltaTime);

        hungerBar.fillAmount = hungerValue;
        thirstBar.fillAmount = thirstValue;
    }

    /// <summary>
    /// Updates the hunger value of the player and sets the UI
    /// </summary>
    /// <param name="amount">how much should be add or be taken away</param>
    public void UpdateHunger(float amount)
    {
        hungerValue = Mathf.Clamp01(hungerValue + amount);
        hungerBar.fillAmount = hungerValue;
    }

    /// <summary>
    /// Updates the thirst value of the player and sets the UI
    /// </summary>
    /// <param name="amount">how much should be add or be taken away</param>
    public void UpdateThirst(float amount)
    {
        thirstValue = Mathf.Clamp01(thirstValue + amount);
        thirstBar.fillAmount = thirstValue;
    }
}
