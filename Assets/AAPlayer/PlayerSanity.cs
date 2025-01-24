using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSanity : MonoBehaviour
{
    /// <summary>
    /// the current sanity level of the player
    /// </summary>
    public float currSanityLevel;

    [SerializeField]
    Image sanityBarFillImage;

    #region Singleton
    public static PlayerSanity Instance { get; private set; }

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
        sanityBarFillImage.fillAmount = currSanityLevel; 
    }

    /// <summary>
    /// Updates the value of the sanity level and correspond to the UI
    /// </summary>
    /// <param name="value">How much Sanity should be applied or taken away</param>
    public void UpdateSanityValue(float value)
    {
        currSanityLevel = Mathf.Clamp01(currSanityLevel + value);

        sanityBarFillImage.fillAmount = currSanityLevel;
    }
}
