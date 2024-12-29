using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCamera : MonoBehaviour
{
    public Transform cameraTransform;

    private void Start()
    {
        transform.SetParent(cameraTransform);
        transform.localPosition = new Vector3(0.1475716f, -0.7f, 1.61f);
        transform.localRotation = Quaternion.identity;
    }

}
