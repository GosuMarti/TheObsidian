using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFloor : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToDestroy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("uhu"))
        {
            foreach (GameObject obj in objectsToDestroy)
            {
                if (obj != null)
                {
                    Destroy(obj);
                }
            }
        }
    }
}
