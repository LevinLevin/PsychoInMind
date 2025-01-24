using UnityEngine;

/// <summary>
/// Every Item in the inventory needs the IWeapon interface in order to define its values and purpose for the player
/// </summary>
public interface IWeapon
{
    /// <summary>
    /// Takes ToolData to define its values.
    /// </summary>
    public ToolData Data { get; set; }

    /// <summary>
    /// Does something when the player tries to use it through the inventory
    /// </summary>
    /// <param name="hit">Takes a RaycastHit as parameter to for example apply damage to enemies or objects</param>
    public void Action(RaycastHit hit);
}
