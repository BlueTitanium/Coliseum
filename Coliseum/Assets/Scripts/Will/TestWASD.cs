using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWASD : MonoBehaviour
{
    public Rigidbody2D rb;
    [SerializeField]
    float force;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w")) {
            rb.AddForce(new Vector2(0, force));
        }
        if (Input.GetKey("a")) {
            rb.AddForce(new Vector2(-force, 0));
        }
        if (Input.GetKey("s")) {
            rb.AddForce(new Vector2(0, -force));
        }
        if (Input.GetKey("d")) {
            rb.AddForce(new Vector2(force, 0));
        }
    }
}
