using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    public string weaponName;
    public int damage;
    public float fireRate;
    public int ammoCapacity;
    public int currentAmmo;

    [Header("Shooting Mechanics")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public int bulletsPerShot = 1;
    public int bulletsPerClick = 1;
    public float bulletSpeed = 20f;

    [Header("Recoil Settings")]
    public float recoilAmount = 0.1f;
    public float recoilRecoverySpeed = 5f;
    private Vector3 originalPosition;

    [Header("Audio & VFX")]
    public AudioClip fireSound; // 🔊 Fire sound
    private AudioSource audioSource;
    public GameObject muzzleFlashPrefab; // 🔥 Muzzle Flash Effect

    [Header("Other Settings")]
    public bool canShoot = true; // ✅ Made Public for WeaponManager Access

    private List<IWeaponObserver> observers = new List<IWeaponObserver>();

    private void Start()
    {
        currentAmmo = ammoCapacity;
        originalPosition = transform.localPosition;
        audioSource = gameObject.AddComponent<AudioSource>(); // Ensure AudioSource exists
        NotifyAmmoChanged();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    public virtual void Shoot()
    {
        if (currentAmmo >= bulletsPerShot && canShoot)
        {
            currentAmmo -= bulletsPerShot;
            ShootBullets();
            ApplyRecoil();
            PlayFireSound();
            ShowMuzzleFlash();
            StartCoroutine(FireRateCooldown());

            Debug.Log($"{weaponName} fired! Remaining ammo: {currentAmmo}");
            NotifyAmmoChanged();
        }
        else
        {
            Debug.Log($"{weaponName} is out of ammo! Reload needed.");
        }
    }

    protected virtual void ShootBullets()
    {
        for (int i = 0; i < bulletsPerShot; i++)
        {
            GameObject bulletObject = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = bulletObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.useGravity = false;
                rb.AddForce(firePoint.forward * bulletSpeed, ForceMode.Impulse);
            }

            Bullet bullet = bulletObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.SetDamage(damage);
            }
        }
    }

    private void PlayFireSound()
    {
        if (fireSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(fireSound);
        }
    }

    private void ShowMuzzleFlash()
    {
        if (muzzleFlashPrefab != null)
        {
            GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation, firePoint);
            muzzleFlash.transform.localScale = new Vector3(
                Random.Range(0.7f, 1.3f),
                Random.Range(0.7f, 1.3f),
                Random.Range(0.7f, 1.3f)
            ); // Makes muzzle flash look more random

            Destroy(muzzleFlash, 0.07f); // Destroy after a very short duration
        }
    }

    public virtual void Reload()
    {
        Debug.Log($"Reloading {weaponName}");
        currentAmmo = ammoCapacity;
        NotifyAmmoChanged();
    }

    IEnumerator FireRateCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(1 / fireRate);
        canShoot = true;
        Debug.Log($"✅ {weaponName} can now shoot again!");
    }

    private void ApplyRecoil()
    {
        transform.localPosition -= new Vector3(0, 0, recoilAmount);
        StartCoroutine(RecoilRecovery());
    }

    private IEnumerator RecoilRecovery()
    {
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, elapsedTime);
            elapsedTime += Time.deltaTime * recoilRecoverySpeed;
            yield return null;
        }
        transform.localPosition = originalPosition;
    }

    public void AddObserver(IWeaponObserver observer)
    {
        if (!observers.Contains(observer))
            observers.Add(observer);
    }

    public void RemoveObserver(IWeaponObserver observer)
    {
        if (observers.Contains(observer))
            observers.Remove(observer);
    }

    private void NotifyAmmoChanged()
    {
        foreach (var observer in observers)
        {
            observer.OnAmmoChanged(currentAmmo, ammoCapacity);
        }
    }
}
