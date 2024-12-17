using UnityEngine;

[CreateAssetMenu(fileName = "Weapons", menuName = "ScriptableObjects/Weapons", order = 1)]
public class ToolData : ScriptableObject
{
    /// <summary>
    /// Name of the Tool or Weapon
    /// </summary>
    [Tooltip("Name of the Tool")]
    public string ToolName;

    /// <summary>
    /// Has to be true if the Tool can kill enemies
    /// </summary>
    [Tooltip("Can the tool kill enemies?")]
    public bool IsWeapon;

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
    [Tooltip("How big is the magazine?")]
    public int MagazineSize;

    public int currentAmmo;
}
