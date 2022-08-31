using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.SetPositionAndRotation(target.position, target.rotation);
    }
}
