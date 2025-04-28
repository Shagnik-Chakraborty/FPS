using UnityEngine;

public class Pistol : Weapon
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse click
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload(); // Calls Reload() from Weapon class
        }
    }

    public override void Shoot()
    {
        if (currentAmmo > 0 && canShoot)
        {
            base.Shoot(); // Calls the base Shoot() method
            Debug.Log("Pistol fired!");
        }
        else
        {
            Debug.Log("Pistol is out of ammo! Reload needed.");
        }
    }
}
