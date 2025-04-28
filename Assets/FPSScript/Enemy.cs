using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public int health = 100; // Can be changed in Inspector
    public float speed = 2f; // Movement speed (0 means no movement)

    private float leftLimit = -20f;
    private float rightLimit = 20f;
    private int direction = 1; // 1 for right, -1 for left

    private void Update()
    {
        if (speed > 0)
        {
            MoveEnemy();
        }
    }

    private void MoveEnemy()
    {
        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        if (transform.position.x >= rightLimit)
        {
            direction = -1; // Move left
        }
        else if (transform.position.x <= leftLimit)
        {
            direction = 1; // Move right
        }
    }
}
