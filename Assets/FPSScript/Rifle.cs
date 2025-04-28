using UnityEngine;

public class Rifle : Weapon
{
    private void Update()
    {
        if (Input.GetMouseButton(0)) // Hold left mouse button to fire continuously
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    public override void Shoot()
    {
        if (currentAmmo > 0 && canShoot)
        {
            bulletsPerClick = 3; // Rifle fires in bursts (adjustable)
            base.Shoot();
            Debug.Log("Rifle burst fired!");
        }
        else
        {
            Debug.Log("Rifle is out of ammo! Reload needed.");
        }
    }
}
