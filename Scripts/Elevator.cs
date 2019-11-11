using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    // Movement vector
    public Vector3 moveDir;
    public LayerMask layerMask;



    void FixedUpdate()
    {
        // Moving
        transform.Translate(moveDir * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // In Elevator
        if (((1 << collision.gameObject.layer) & layerMask) != 0)
        {
            Debug.Log(collision.gameObject.name + " is on.");
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        // Moving with Elevator : Same code with the code defined elevator's moving part
        if (((1 << collision.gameObject.layer) & layerMask) != 0)
        {
            collision.transform.position += moveDir * Time.deltaTime;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        // Out Elevator
        if (((1 << collision.gameObject.layer) & layerMask) != 0)
        {
            Debug.Log(collision.gameObject.name + " is off.");
        }
    }
}
