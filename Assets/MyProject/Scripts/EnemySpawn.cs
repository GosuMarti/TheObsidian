using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject skeletonPrefab;
    [SerializeField] private Transform spawnPoint1;
    [SerializeField] private Transform spawnPoint2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("uhu"))
        {
            if (skeletonPrefab != null)
            {
                // Spawn at first point
                if (spawnPoint1 != null)
                {
                    Instantiate(skeletonPrefab, spawnPoint1.position, spawnPoint1.rotation);
                }

                // Spawn at second point
                if (spawnPoint2 != null)
                {
                    Instantiate(skeletonPrefab, spawnPoint2.position, spawnPoint2.rotation);
                }
            }
        }
    }
}
