using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage;
    private bool hasHitEnemy = false;

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    public int GetDamage()
    {
        return damage;
    }

    private void Start()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet"); // Find all bullets in the scene
        Collider bulletCollider = GetComponent<Collider>();

        foreach (GameObject otherBullet in bullets)
        {
            if (otherBullet != gameObject) // Avoid ignoring collision with itself
            {
                Collider otherCollider = otherBullet.GetComponent<Collider>();
                if (otherCollider != null)
                {
                    Physics.IgnoreCollision(bulletCollider, otherCollider);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Bullet collided with: " + collision.gameObject.name);

        if (!hasHitEnemy && collision.gameObject.CompareTag("Enemy"))
        {
            hasHitEnemy = true; // ✅ Prevent multiple hits

            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                int bulletDamage = GetDamage();
                Debug.Log($"✅ Bullet dealing {bulletDamage} damage to {enemy.gameObject.name} (Before: {enemy.health})");

                enemy.health -= bulletDamage;

                Debug.Log($"💀 Enemy Hit! New Health: {enemy.health}");

                if (enemy.health <= 0)
                {
                    Debug.Log("💥 Enemy Destroyed!");
                    Destroy(enemy.gameObject);
                }
            }
        }

        Destroy(gameObject); // ✅ Destroy bullet on impact
    }
}
