using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private const KeyCode InteractKeyE = KeyCode.E;
    private const KeyCode InteractKeyQ = KeyCode.Q;
    private const KeyCode ShootKey = KeyCode.Mouse0;

    private PlayerHealth playerHealth;
    private HungerThirstManager hungerThirstManager;
    private ObjectPooler pooler;


    private int toolIndex;
    private int goToolIndex;
    private IWeapon currWeapon;

    private IInteractable interactable;

    [Header("Inventory Objects")]
    public GameObject[] toolObjects; //all the objects in the inventory
    public List<IWeapon> toolObjectList = new List<IWeapon>();
    public SaveObject saveObject;

    [Header("Player Camera")]
    [SerializeField]
    Transform PlayerCam;

    [Header("Inventory Canvas")]
    [SerializeField]
    GameObject InventoryCanvas;

    [SerializeField]
    TMP_Text[] captions;

    [Header("Interaction Canvas")]
    [SerializeField]
    GameObject optionE;
    [SerializeField]
    GameObject optionQ;
    /// <summary>
    /// Q
    /// </summary>
    [SerializeField]
    TMP_Text Option1;
    /// <summary>
    /// E
    /// </summary>
    [SerializeField]
    TMP_Text Option2;

    #region Singelton
    public static PlayerInventory Instance { get; private set; }

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

        LoadToolObjectList();
    }
    #endregion

    private void Start()
    {
        playerHealth = PlayerHealth.Instance;
        hungerThirstManager = HungerThirstManager.Instance;
        pooler = ObjectPooler.Instance;

        toolIndex = 0;

        foreach (GameObject tool in toolObjects)
        {
            tool.SetActive(false);
        }
        toolObjects[toolIndex].SetActive(true);
        currWeapon = toolObjects[toolIndex].GetComponent<IWeapon>();

        optionQ.SetActive(false);
        optionE.SetActive(false);

        UpdateAmmo();
    }

    private void Update()
    {
        optionE.SetActive(false);
        optionQ.SetActive(false);

        if (currWeapon != null)
        {
            Debug.DrawRay(PlayerCam.transform.position, PlayerCam.transform.forward * currWeapon.Data.Distance, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(PlayerCam.transform.position, PlayerCam.transform.forward, out hit, 1.9f)) //are we close enough
            {
                if (hit.collider.TryGetComponent(out IInteractable interact)) //is it a interactable we see ?
                {
                    interactable = interact;
                    if (!string.IsNullOrWhiteSpace(interactable.txtOptionE())) //does ist have the option e ?
                    {
                        Option2.text = interactable.txtOptionE();
                        optionE.SetActive(true);

                        if (Input.GetKeyDown(InteractKeyE))
                        {
                            interactable.InteractE();
                            //Update the texts
                            Option2.text = interactable.txtOptionE();
                            Option1.text = interactable.txtOptionQ();
                            optionE.SetActive(false);
                            optionQ.SetActive(false);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(interactable.txtOptionQ()))
                    {
                        Option1.text = interactable.txtOptionQ();
                        optionQ.SetActive(true);

                        if (Input.GetKeyDown(InteractKeyQ))
                        {
                            interactable.InteractQ();
                            //update the text
                            Option2.text = interactable.txtOptionE();
                            Option1.text = interactable.txtOptionQ();
                            optionE.SetActive(false);
                            optionQ.SetActive(false);
                        }
                    }
                }
            }
        }
        if (currWeapon.Data.IsWeapon)
        {
            if (Input.GetKeyDown(ShootKey))
            {
                Shoot();
                CheckForItemStorage();
            }
        }
        else if (currWeapon.Data.IsConsumable)
        {
            if (Input.GetKeyDown(ShootKey))
            {
                ConsumeItem();
                CheckForItemStorage();
            }
        }

        #region Switch Input

        if (Input.GetKey(KeyCode.Tab))
        {
            InventoryCanvas.SetActive(true);
        }
        else
        {
            InventoryCanvas.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Pistol
            goToolIndex = 0;
            SwitchItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //Axe
            goToolIndex = 1;
            SwitchItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //Pickaxe
            goToolIndex = 2;
            SwitchItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //Stick
            goToolIndex = 3;
            SwitchItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            //CookedMeat
            goToolIndex = 4;
            SwitchItem();
        }
        else
        {

        }
        #endregion
    }

    private void SwitchItem()
    {
        // Check if goToolIndex is within bounds
        if (goToolIndex < 0 || goToolIndex >= toolObjects.Length)
        {
            Debug.LogError("Invalid tool index.");
            return;
        }

        // Get the weapon component of the selected tool
        var goToTool = toolObjects[goToolIndex];
        if (goToTool == null)
        {
            Debug.LogError("Tool object at index is null.");
            return;
        }

        var goToWeapon = toolObjectList[goToolIndex];
        if (goToWeapon == null || goToWeapon.Data.Count <= 0) // Assuming IWeapon has Count
        {
            Debug.LogWarning("Invalid or empty weapon.");
            return;
        }

        toolIndex = goToolIndex;

        foreach (GameObject tool in toolObjects)
        {
            tool.SetActive(false);
        }

        toolObjects[toolIndex].SetActive(true);
        currWeapon = toolObjects[toolIndex].GetComponent<IWeapon>();
    }

    private void CheckForItemStorage()
    {
        if (currWeapon.Data.Count <= 0)
        {
            //when the resource is used and its gone now. it cant be selected anymore so by default the gun is selected
            goToolIndex = 0;
            SwitchItem();
        }
    }
    private void Shoot()
    {
        Ray ray = new Ray(PlayerCam.transform.position, PlayerCam.transform.forward);
        RaycastHit hit1;
        if(Physics.Raycast(ray, out hit1, currWeapon.Data.Distance))
        {
            if(currWeapon.Data.needMagazine && currWeapon.Data.currentAmmo > 0)
            {
                //hitPrefabLogic
                float positionMultiplier = 0.1f;
                float spawnX = hit1.point.x - ray.direction.x * positionMultiplier;
                float spawnY = hit1.point.y - ray.direction.y * positionMultiplier;
                float spawnZ = hit1.point.z - ray.direction.z * positionMultiplier;
                Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);

                GameObject spawnedObject = pooler.SpawnFromPool(currWeapon.Data.hitPrefab, spawnPosition, Quaternion.identity);
                Quaternion targetRotation = Quaternion.LookRotation(ray.direction);

                spawnedObject.transform.rotation = targetRotation;
            }
            else if(!currWeapon.Data.needMagazine)
            {
                //hitPrefabLogic
                float positionMultiplier = 0.1f;
                float spawnX = hit1.point.x - ray.direction.x * positionMultiplier;
                float spawnY = hit1.point.y - ray.direction.y * positionMultiplier;
                float spawnZ = hit1.point.z - ray.direction.z * positionMultiplier;
                Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);

                GameObject spawnedObject = pooler.SpawnFromPool(currWeapon.Data.hitPrefab, spawnPosition, Quaternion.identity);
                Quaternion targetRotation = Quaternion.LookRotation(ray.direction);

                spawnedObject.transform.rotation = targetRotation;
            }
            
        }

        currWeapon.Action(hit1);
    }

    private void ConsumeItem()
    {
        //the count of the object decreases
        currWeapon.Data.Count--;
        playerHealth.UpdateLifeValue(0.2f);
        hungerThirstManager.UpdateHunger(0.5f);
    }

    /// <summary>
    /// Can be called when the player collects a specific item for the inventory
    /// </summary>
    /// <param name="itemName">Takes a ItemEnum to compare it to the other Items in the inventory</param>
    public void CollectItem(ItemEnum itemName)
    {
        foreach (IWeapon item in toolObjectList)
        {
            if(item.Data.ToolName == itemName)
            {
                item.Data.Count++;
                UpdateAmmo();
            }
        }
    }

    private void UpdateAmmo()
    {
        foreach (IWeapon weapon in toolObjectList)
        {
            if (weapon.Data.ToolName == ItemEnum.Ammo)
            {
                foreach (GameObject item2 in toolObjects)
                {
                    if (item2.TryGetComponent(out IWeapon pistol))
                    {
                        if (pistol.Data.ToolName == ItemEnum.Pistol)
                        {
                            int ammoCount = weapon.Data.Count;
                            pistol.Data.Ammo += weapon.Data.Count;
                            weapon.Data.Count -= ammoCount;
                        }
                    }
                }
            }
        }
    }

    private void LoadToolObjectList()
    {
        foreach (GameObject tool in toolObjects)
        {
            if (tool.TryGetComponent(out IWeapon weapon))
            {
                toolObjectList.Add(weapon);
            }
        }
    }

    #region Save and Load Methods

    /// <summary>
    /// Saves all the specific values of different tools in the SaveManagers saveObject
    /// </summary>
    public void SaveInventoryState()
    {
        SaveManager.Instance.saveObject.pistolCurrentAmmo = toolObjectList[0].Data.currentAmmo; // Assuming index 0 is pistol
        SaveManager.Instance.saveObject.pistolAmmo = toolObjectList[0].Data.Ammo;
        SaveManager.Instance.saveObject.woodCount = toolObjectList[3].Data.Count;
        SaveManager.Instance.saveObject.cookedMeatCount = toolObjectList[4].Data.Count;
        SaveManager.Instance.saveObject.ironCount = toolObjectList[5].Data.Count;
        SaveManager.Instance.saveObject.fleshCount = toolObjectList[6].Data.Count;
    }

    /// <summary>
    /// Loads all the specific values of different tools from the SaveManagers saveObject
    /// </summary>
    public void LoadInventoryState()
    {
        if (saveObject != null)
        {
            toolObjectList[0].Data.currentAmmo = SaveManager.Instance.saveObject.pistolCurrentAmmo;
            toolObjectList[0].Data.Ammo = SaveManager.Instance.saveObject.pistolAmmo;
            toolObjectList[3].Data.Count = SaveManager.Instance.saveObject.woodCount;
            toolObjectList[4].Data.Count = SaveManager.Instance.saveObject.cookedMeatCount;
            toolObjectList[5].Data.Count = SaveManager.Instance.saveObject.ironCount;
            toolObjectList[6].Data.Count = SaveManager.Instance.saveObject.fleshCount;
        }
    }

    #endregion
}
