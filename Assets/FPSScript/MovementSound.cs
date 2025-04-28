using UnityEngine;

public class MovementSound : MonoBehaviour
{
    public AudioSource movementAudio; // Assign an AudioSource in the inspector
    public AudioClip movementClip; // Assign a movement sound clip in the inspector
    private bool isMoving = false;

    void Start()
    {
        if (movementAudio == null)
        {
            movementAudio = gameObject.AddComponent<AudioSource>();
        }

        movementAudio.clip = movementClip;
        movementAudio.loop = true; // Make the sound loop while holding the key
    }

    void Update()
    {
        bool isKeyPressed = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                            Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (isKeyPressed && !isMoving)
        {
            movementAudio.Play();
            isMoving = true;
        }
        else if (!isKeyPressed && isMoving)
        {
            movementAudio.Stop();
            isMoving = false;
        }
    }
}
