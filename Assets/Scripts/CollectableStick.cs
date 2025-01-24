using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lets the Player collect or make fire with this script on
/// </summary>
public class CollectableStick : MonoBehaviour, IInteractable
{
    bool isFire;

    bool wantFire;

    int woodCount = 0;

    private float initialIntensity = 20f;
    private float targetIntensity = 0f;


    PlayerInventory inventory;
    ObjectPooler pooler;

    [SerializeField]
    private ParticleSystem fireParticle;

    [SerializeField]
    private Light lightSource;

    [SerializeField]
    private float fireDuration;

    [SerializeField]
    private float resourceSpawnOffset;

    private Dictionary<ItemEnum, GameObject> itemCache;


    private void InitializeCache()
    {
        itemCache = new Dictionary<ItemEnum, GameObject>();

        foreach (GameObject item in inventory.toolObjects)
        {
            if (item.TryGetComponent(out IWeapon weapon))
            {
                if (!itemCache.ContainsKey(weapon.Data.ToolName))
                {
                    itemCache[weapon.Data.ToolName] = item;
                }
            }
        }
    }

    private void Start()
    {
        inventory = PlayerInventory.Instance;
        pooler = ObjectPooler.Instance;

        InitializeCache();

        isFire = false;
        wantFire = false;
        lightSource.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        isFire = false;
        wantFire = false;
        lightSource.gameObject.SetActive(false);
    }

    public void InteractE()
    {
        if (!isFire && !wantFire)
        {
            Debug.Log("Collect Stick");
            //when the first action is E, collect stick
            inventory.CollectItem(ItemEnum.Stick);
            gameObject.SetActive(false);
        }
        else if (!isFire && wantFire && IsAvailable(ItemEnum.Stone))
        {
            Debug.Log("Want Fire for Iron");

            //when the second action is E, make iron with stone
            isFire = true;
            woodCount++;

            //Reduce the used items
            ReduceItem(ItemEnum.Stick);
            ReduceItem(ItemEnum.Stone);
            UpdateCache();

            StartCoroutine(Fire(ItemEnum.Ammo));
        }
    }

    public void InteractQ()
    {
        if(!isFire && !wantFire && CanMakeFire())
        {
            Debug.Log("Want Fire");

            wantFire = true;
        }
        else if (!isFire && wantFire && IsAvailable(ItemEnum.Flesh))
        {
            Debug.Log("Want Fire for cooked meat");

            //when the second action is Q, make cookedmeat with flesh
            isFire = true;
            woodCount++;

            //Reduce the used items
            ReduceItem(ItemEnum.Stick);
            ReduceItem(ItemEnum.Flesh);
            UpdateCache();

            StartCoroutine(Fire(ItemEnum.CookedMeat));
        }
        else if(isFire && wantFire && IsAvailable(ItemEnum.Stick))
        {
            Debug.Log("Wood is added to the fire");
            woodCount++;
            ReduceItem(ItemEnum.Stick);
        }
    }

    private bool IsAvailable(ItemEnum availableItem)
    {
        if (itemCache.TryGetValue(availableItem, out GameObject item))
        {
            if (item.TryGetComponent(out IWeapon weapon) && weapon.Data.Count > 0)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if there are nesseccary items in the inventory to make fire (sticks, flesh or iron)
    /// </summary>
    /// <returns></returns>
    public bool CanMakeFire()
    {
        if((IsAvailable(ItemEnum.Flesh)|| IsAvailable(ItemEnum.Stone)) && IsAvailable(ItemEnum.Stick))
        {
            return true;
        }
        return false;
    }

    private void ReduceItem(ItemEnum reductedItem)
    {
        if (itemCache.TryGetValue(reductedItem, out GameObject item))
        {
            if (item.TryGetComponent(out IWeapon weapon) && weapon.Data.Count > 0)
            {
                weapon.Data.Count--;
            }
        }
    }

    public string txtOptionE()
    {
        if (!isFire && !wantFire)
        {
            return "CollectItem";
        }
        else if (!isFire && wantFire && IsAvailable(ItemEnum.Stone))
        {
            return "Ammo";
        }
        else
        {
            return null;
        }
    }

    public string txtOptionQ()
    {
        if (!isFire && !wantFire && CanMakeFire())
        {
            return "Make Fire";
        }
        else if(!isFire && wantFire && IsAvailable(ItemEnum.Flesh))
        {
            return "Cooked Meat";
        }
        else if(isFire && wantFire && IsAvailable(ItemEnum.Stick))
        {
            return "Add Wood";
        }
        else
        {
            return null;
        }
    }

    private IEnumerator Fire(ItemEnum finalProduct)
    {
        transform.rotation = Quaternion.Euler(-90f, transform.rotation.eulerAngles.y, 0f);
        fireParticle.Play();
        lightSource.gameObject.SetActive(true);
        do
        {
            yield return new WaitForSeconds(fireDuration);

            pooler.SpawnFromPool(finalProduct, new Vector3(transform.position.x, transform.position.y + resourceSpawnOffset, transform.position.z), Quaternion.identity);
            woodCount--;
        }
        while (woodCount > 0);

        fireParticle.Stop();

        bool lightOut = false;
        float dimValue = 0f;
        while (!lightOut)
        {
            lightSource.intensity = Mathf.Lerp(initialIntensity, targetIntensity, dimValue);
            dimValue += 0.1f;
            if(dimValue > 1f)
            {
                lightOut = true;
            }
            yield return new WaitForSeconds(0.1f);
        }
        DeactivateFireBranch();
    }

    private void DeactivateFireBranch()
    {
        isFire = false;
        wantFire = false;
        lightSource.intensity = 20f;
        lightSource.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void UpdateCache()
    {
        itemCache.Clear();

        foreach (GameObject item in inventory.toolObjects)
        {
            if (item.TryGetComponent(out IWeapon weapon))
            {
                if (!itemCache.ContainsKey(weapon.Data.ToolName))
                {
                    itemCache[weapon.Data.ToolName] = item;
                }
            }
        }
    }
}
