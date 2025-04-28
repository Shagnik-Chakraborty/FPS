using UnityEngine;

public class Shotgun : Weapon
{
    [Header("Shotgun Settings")]
    public float spreadAngle = 10f; // Adjust in Inspector for wider/narrower spread

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Single shot per click
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    // ? Keep it 'protected override' to match the base class
    protected override void ShootBullets()
    {
        for (int i = 0; i < bulletsPerShot; i++)
        {
            Vector3 spread = new Vector3(
                Random.Range(-spreadAngle, spreadAngle),
                Random.Range(-spreadAngle, spreadAngle),
                0
            );

            Quaternion bulletRotation = Quaternion.Euler(firePoint.eulerAngles + spread);
            GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, bulletRotation);

            Bullet bulletScript = bulletObj.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDamage(damage); // ? Ensure damage is set correctly
            }

            Rigidbody rb = bulletObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = bulletObj.transform.forward * bulletSpeed;
            }
        }
    }



}
