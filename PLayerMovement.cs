using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    float turnSpeed = 100.0f;
    float moveSpeed = 15.0f;
    void Start()
    {

    }
    void Update()
    {
        // transform.Rotate(Vector3.up * turnSpeed * Input.GetAxis("Horizontal") * Time.deltaTime);
        transform.Translate(0f, 0f, moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
    }
}
