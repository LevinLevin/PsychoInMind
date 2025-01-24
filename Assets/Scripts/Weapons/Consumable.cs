using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour, IWeapon
{
    [SerializeField]
    private ToolData _toolData;

    public ToolData Data
    {
        get => _toolData;
        set => _toolData = value;
    }

    public void Action(RaycastHit hit)
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {
        Data = _toolData;
    }
}
