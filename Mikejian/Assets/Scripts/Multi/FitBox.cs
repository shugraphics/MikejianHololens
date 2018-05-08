using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitBox : MonoBehaviour {
    private void LateUpdate()
    {
        Transform CameraTransform = Camera.main.transform;
        transform.position = CameraTransform.position + (CameraTransform.forward * 20);
        transform.rotation = Quaternion.LookRotation(CameraTransform.forward, CameraTransform.up);
    }
}
