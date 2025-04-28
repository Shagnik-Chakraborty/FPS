using UnityEngine;
using System; // ✅ Add this for Action events
using TMPro;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }  // Singleton Instance
    public TMP_Text weaponNameText;

    public Weapon currentWeapon; // Reference to the currently equipped weapon
    public Weapon[] weapons; // Array to hold all available weapons
    private int currentWeaponIndex = 0;

    // ✅ Event to notify observers when a weapon is switched
    public event Action<Weapon> OnWeaponSwitched;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
            return;
        }
        DontDestroyOnLoad(gameObject); // Optional: Keeps the manager alive across scenes

        if (weapons.Length > 0)
        {
            EquipWeapon(0); // Equip the first weapon by default
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipWeapon(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) EquipWeapon(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) EquipWeapon(2);
        else if (Input.mouseScrollDelta.y != 0) ScrollWeapons((int)Input.mouseScrollDelta.y);
    }

    public void EquipWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length) return;

        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(false);
        }

        currentWeapon = weapons[index];
        currentWeapon.gameObject.SetActive(true);
        currentWeaponIndex = index;

        Debug.Log($"🔫 Equipped {currentWeapon.weaponName}");

        // ✅ Reset shooting state
        currentWeapon.canShoot = true;  // Ensure weapon can shoot after switching

        // ✅ Notify observers that the weapon has changed
        OnWeaponSwitched?.Invoke(currentWeapon);

        // ✅ Update the UI with the new weapon name
        if (weaponNameText != null)
        {
            weaponNameText.text = currentWeapon.weaponName;
        }
    }



    private void ScrollWeapons(int direction)
    {
        int newIndex = (currentWeaponIndex + direction) % weapons.Length;
        if (newIndex < 0) newIndex = weapons.Length - 1;
        EquipWeapon(newIndex);
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
