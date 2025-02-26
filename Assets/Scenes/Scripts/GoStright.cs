using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoStright : MonoBehaviour
{
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.right * Random.Range(50, 100), ForceMode.VelocityChange);
    }
}
