using UnityEngine;

public class PlayerInventory : MonoBehaviour, ICollectItems
{
    public int toolIndex;
    public ToolData currTool;

    [Header("Tools in Inventory")]
    public ToolData[] tools;

    public GameObject[] toolObjects;

    [Header("Items in Inventory")]
    public ItemData[] items;

    [SerializeField]
    Transform Camera;

    [SerializeField]
    GameObject InventoryCanvas;

    // Start is called before the first frame update
    void Start()
    {
        toolIndex = 0;

        foreach (GameObject tool in toolObjects)
        {
            tool.SetActive(false);
        }
        toolObjects[toolIndex].SetActive(true);
        currTool = tools[toolIndex];
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (currTool.needMagazine)
                ReloadTool();
        }

        if (Input.GetKey(KeyCode.E))
        {
            InventoryCanvas.SetActive(true);
        }
        else
        {
            InventoryCanvas.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            toolIndex = 0;
            SwitchTool();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            toolIndex = 1;
            SwitchTool();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            toolIndex = 2;
            SwitchTool();
        }
    }

    void Shoot()
    {
        if (currTool.needMagazine && currTool.currentAmmo > 0)
        {
            currTool.currentAmmo --;
            RaycastHit hit;
            if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hit, currTool.Distance))
            {
                Debug.DrawRay(Camera.transform.position, Camera.transform.forward * currTool.Distance, Color.red);

                if (((1 << hit.transform.gameObject.layer) & currTool.RescourceLayer.value) != 0)
                {
                    if (hit.collider.TryGetComponent(out ITakeDamage Damage))
                    {
                        Damage.TakeDamage(currTool.Damage);
                    }
                }
            }
        }
        else if(!currTool.needMagazine)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hit, currTool.Distance))
            {
                Debug.DrawRay(Camera.transform.position, Camera.transform.forward * currTool.Distance, Color.red);

                if (((1 << hit.transform.gameObject.layer) & currTool.RescourceLayer.value) != 0)
                {
                    Debug.Log("SameLayer!");
                    if (hit.collider.TryGetComponent(out ITakeDamage Damage))
                    {
                        Damage.TakeDamage(currTool.Damage);
                    }
                }
            }
        }
    }

    void ReloadTool()
    {
        currTool.currentAmmo = currTool.MagazineSize;
    }

    void SwitchTool()
    {
        switch (toolIndex)
        {
            case 0:
                foreach (GameObject tool in toolObjects)
                {
                    tool.SetActive(false);
                }
                toolObjects[toolIndex].SetActive(true);
                currTool = tools[toolIndex];
                break;
            case 1:
                foreach (GameObject tool in toolObjects)
                {
                    tool.SetActive(false);
                }
                toolObjects[toolIndex].SetActive(true);
                currTool = tools[toolIndex];
                break;
            case 2:
                foreach (GameObject tool in toolObjects)
                {
                    tool.SetActive(false);
                }
                toolObjects[toolIndex].SetActive(true);
                currTool = tools[toolIndex];
                break;
        }
    }

    public void CollectItem(ItemData itemData)
    {
        foreach (ItemData item in items)
        {
            if (itemData.Name == item.Name)
            {
                item.Count += itemData.Count;
            }
        }
    }
}
