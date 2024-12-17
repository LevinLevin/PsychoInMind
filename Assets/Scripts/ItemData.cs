using UnityEngine;

[CreateAssetMenu(fileName = "Resources", menuName = "ScriptableObjects/Resources", order = 2)]

public class ItemData : ScriptableObject
{
    /// <summary>
    /// The Name always has to be the same so it is comparable
    /// </summary>
    public string Name;

    /// <summary>
    /// Counts how many objects the inventory has to add 
    /// </summary>
    public int Count;
}
