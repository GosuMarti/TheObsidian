using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    public Transform player;
    public KeyCode interactKey = KeyCode.E;
    public float interactRange = 3f;
    public AudioSource audioSource;
    public AudioClip pickupSound;

    private void Update()
    {
        if (player == null)
        {
            Debug.LogError("Player reference not assigned!");
            return;
        }

        // Check distance and input key
        if (Vector3.Distance(transform.position, player.position) <= interactRange &&
            Input.GetKeyDown(interactKey))
        {
            Pickup();
        }
    }

    private void Pickup()
    {
        Debug.Log("Picked up: " + gameObject.name);

        // Create a temporary object for sound
        GameObject tempAudioObject = new GameObject("PickupSound");
        AudioSource tempAudioSource = tempAudioObject.AddComponent<AudioSource>();
        tempAudioSource.clip = pickupSound;
        tempAudioSource.loop = false; // Ensure it's not looping
        tempAudioSource.Play();

        // Destroy the temporary audio object after the clip duration
        Destroy(tempAudioObject, pickupSound.length);

        // Destroy the main object
        Destroy(gameObject);
    }
}
