using System.Diagnostics;
using UnityEngine;

/// <summary>
/// Defines all the values a inventory item needs
/// </summary>
[CreateAssetMenu(fileName = "InventoryItem", menuName = "Inventory/InventoryItem", order = 1)]
public class ToolData : ScriptableObject
{
    /// <summary>
    /// Name of the Tool or Weapon
    /// </summary>
    [Tooltip("Name of the Tool")]
    public ItemEnum ToolName;

    /// <summary>
    /// Nescessary for every Collectable Item
    /// </summary>
    [Tooltip("How many Items are there in the Inventory")]
    public int Count;

    /// <summary>
    /// Has to be true if it can make damage
    /// </summary>
    [Header("Any functionality?")]
    [Tooltip("Can the tool make damage?")]
    public bool IsWeapon;

    /// <summary>
    /// has to be true, to interact with an object
    /// </summary>
    [Tooltip("Does it Interact with anything?")]
    public bool IsInteractable;

    /// <summary>
    /// Is only true, if it doesnt make damage or cant be interacted with
    /// </summary>
    [Tooltip("Can the Item be consumed?")]
    public bool IsConsumable;

    /// <summary>
    /// From how far can the target be hit
    /// </summary>
    [Tooltip("How far does the tool/weapon reach?")]
    public float Distance;

    /// <summary>
    /// How much damage deals the Tool
    /// </summary>
    [Tooltip("How much damage deals the weapon?")]
    [Range(0.0f, 1.0f)]
    public float Damage;

    /// <summary>
    /// The visible texture, when the tool hits something
    /// </summary>
    public ItemEnum hitPrefab;

    /// <summary>
    /// How long do you have to wait until hit again
    /// </summary>
    [Tooltip("Weapon speed?")]
    public float Cooldown;

    /// <summary>
    /// What Layer has the Rescource to be in order to be damageable
    /// </summary>
    [Tooltip("Layer of the Target Object e.g.Enemy")]
    public LayerMask RescourceLayer;

    /// <summary>
    /// Does the Tool has a magazine that needs refill
    /// </summary>
    [Tooltip("Tool has a magazine?")]
    public bool needMagazine;

    /// <summary>
    /// Weapons need a Magazine and it decides how many bullets can be fired before you have to reload
    /// </summary>
    [Header("Need Magazine?")]
    [Tooltip("How big is the magazine?")]
    public int MagazineSize;

    /// <summary>
    /// Is the Ammo in the Gun
    /// </summary>
    public int currentAmmo;

    /// <summary>
    /// Is the Ammo in the Inventory
    /// </summary>
    public int Ammo;

    /// <summary>
    /// The time it takes to reload the Magazine
    /// </summary>
    public float ReloadTime;
}
