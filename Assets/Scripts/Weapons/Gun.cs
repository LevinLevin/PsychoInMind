using System.Collections;
using UnityEngine;

/// <summary>
/// Is used for Weapons and Tools in the inventory. It speaks to Objects that have the ITakeDamage interface.
/// </summary>
public class Gun : MonoBehaviour, IWeapon
{
    private Collider col;

    private bool isReloading = false;

    [SerializeField]
    private ToolData _toolData;

    public ToolData Data
    {
        get => _toolData;
        set => _toolData = value;
    }


    private float lastShootTime = -Mathf.Infinity;

    private void Start()
    {
        Data = _toolData;
    }

    private void Update()
    {
        if (Data.needMagazine && !isReloading)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(Reload());
            }
        }
    }

    public void Action(RaycastHit hit)
    {
        if (isReloading)
            return;

        if (Time.time - lastShootTime < Data.Cooldown)
            return;

        if (hit.collider != null)
            col = hit.collider;

        //A shot was fired despite the fact nothing was hit
        lastShootTime = Time.time;

        if (Data.needMagazine && Data.currentAmmo > 0)
        {
            Data.currentAmmo--;

            if (col == null)
                return;

            if (col.CompareTag("EnemyHead"))
            {
                hit.collider.gameObject.GetComponentInParent<ITakeDamage>().TakeDamage(Data.Damage * 2f);
            }
            else if (col.CompareTag("EnemyBody"))
            {
                hit.collider.gameObject.GetComponentInParent<ITakeDamage>().TakeDamage(Data.Damage);
            }
            else if (col.CompareTag("EnemyArmor"))
            {
                hit.collider.gameObject.GetComponentInParent<ITakeDamage>().TakeDamage(Data.Damage * 0.5f);
            }
            else if (col.CompareTag("EnemyCrit"))
            {
                hit.collider.gameObject.GetComponentInParent<ITakeDamage>().TakeDamage(Data.Damage * 5f);
            }

            if (Data.currentAmmo <=0 || Data.Ammo > 0)
            {
                Reload();
            }
        }
        else if(!Data.needMagazine && col != null)
        {
            if (((1 << col.gameObject.layer) & Data.RescourceLayer.value) != 0)
            {
                if (col.TryGetComponent(out ITakeDamage Damage))
                {
                    Damage.TakeDamage(Data.Damage);
                }
            }
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        int ammoNeeded = Data.MagazineSize - Data.currentAmmo; // How much we need to fill the magazine
        int ammoToReload = Mathf.Min(Data.Ammo, ammoNeeded); // Reload only what we have or what we need

        Data.currentAmmo += ammoToReload;
        Data.Ammo -= ammoToReload;

        Debug.Log($"Reloaded {ammoToReload} bullets. Current ammo: {Data.currentAmmo}/{Data.MagazineSize}, Total ammo left: {Data.Ammo}");

        yield return new WaitForSeconds(Data.ReloadTime);
        isReloading = false;
    }
}