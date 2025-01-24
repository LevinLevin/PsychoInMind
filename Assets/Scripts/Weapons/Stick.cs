using UnityEngine;

public class Stick : MonoBehaviour, IWeapon
{
    private ObjectPooler objectPooler;

    [SerializeField]
    private ToolData _toolData;

    public ToolData Data
    {
        get => _toolData;
        set => _toolData = value;
    }

    public void Action(RaycastHit hit)
    {
        Debug.Log("Action");

        Data.Count--;
        objectPooler.SpawnFromPool(ItemEnum.Stick, transform.position, transform.rotation);
    }

    private void Start()
    {
        Data = _toolData;

        objectPooler = ObjectPooler.Instance;
    }
}
