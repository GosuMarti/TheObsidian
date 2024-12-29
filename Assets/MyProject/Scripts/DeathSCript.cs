using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSCript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("uhu"))
        {
            // TODO Show game over screen
        }
    }
}
