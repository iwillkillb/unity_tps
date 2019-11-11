using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiggleBone : MonoBehaviour
{

    Rigidbody _Rigidbody;

    Vector3 prevFrameParentPosition = Vector3.zero;
    public float power = 0f;
    public float clampDist = 0.03f;



    // Start is called before the first frame update
    void Start()
    {
        _Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _Rigidbody.AddForce((prevFrameParentPosition - transform.parent.position) * 100);

        prevFrameParentPosition = transform.parent.position;
    }

}
