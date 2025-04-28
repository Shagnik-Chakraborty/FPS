using UnityEngine;
using TMPro;

public class WeaponUI : MonoBehaviour, IWeaponObserver
{
    public TMP_Text ammoText;

    private void Start()
    {
        WeaponManager.Instance.OnWeaponSwitched += UpdateWeaponObserver;
        UpdateWeaponObserver(WeaponManager.Instance.GetCurrentWeapon()); // Initialize with active weapon
    }

    private void UpdateWeaponObserver(Weapon newWeapon)
    {
        if (newWeapon != null)
        {
            newWeapon.AddObserver(this);
            OnAmmoChanged(newWeapon.currentAmmo, newWeapon.ammoCapacity);
        }
    }

    public void OnAmmoChanged(int currentAmmo, int maxAmmo)
    {
        ammoText.text = $"Ammo: {currentAmmo}/{maxAmmo}";
    }

    private void OnDestroy()
    {
        WeaponManager.Instance.OnWeaponSwitched -= UpdateWeaponObserver;
    }
}
