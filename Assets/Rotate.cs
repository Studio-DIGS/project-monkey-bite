using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotateSpeed = 1f;

    void Update()
    {
        // Rotate the camera around the y-axis based on the horizontal mouse input
        float horizontalInput = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up, horizontalInput * rotateSpeed, Space.World);
    }
}
