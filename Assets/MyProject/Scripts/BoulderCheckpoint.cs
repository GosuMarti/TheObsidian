using UnityEngine;

public class BoulderSpawner : MonoBehaviour
{
    [SerializeField] private GameObject boulderPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Vector3 initialVelocity;
    [SerializeField] private AudioClip spawnSound;
    [SerializeField] private AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("uhu"))
        {
            if (boulderPrefab != null && spawnPoint != null)
            {
                // Spawn the boulder
                GameObject spawnedBoulder = Instantiate(boulderPrefab, spawnPoint.position, spawnPoint.rotation);

                // Apply initial velocity if the boulder has a Rigidbody
                Rigidbody boulder = spawnedBoulder.GetComponent<Rigidbody>();
                if (boulder != null)
                {
                    boulder.velocity = initialVelocity;
                }

                // Play the spawn sound for 11 seconds
                if (audioSource != null && spawnSound != null)
                {
                    audioSource.clip = spawnSound;
                    audioSource.loop = false; // Ensure it's not looping
                    audioSource.Play();

                    // Stop the sound after 11 seconds if needed
                    StartCoroutine(StopSoundAfterTime(11f));
                }
            }
        }
    }

    private System.Collections.IEnumerator StopSoundAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}