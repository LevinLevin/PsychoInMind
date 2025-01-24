using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Item to let the player drink from
/// </summary>
public class InteractableWater : MonoBehaviour, IInteractable
{
    private HungerThirstManager hungerThirstManager;

    private void Start()
    {
        hungerThirstManager = HungerThirstManager.Instance;
    }

    public void InteractE()
    {
        Debug.Log("Player Drinks");
        hungerThirstManager.UpdateThirst(0.5f);
    }

    public void InteractQ()
    {
        throw new System.NotImplementedException();
    }

    public string txtOptionE()
    {
        return "Drink";
    }

    public string txtOptionQ()
    {
        return null;
    }
}
