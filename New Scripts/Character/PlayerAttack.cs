using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float physicsPower = 1000f;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        // Left click
        if (Input.GetMouseButtonDown(0))
        {
            Ray rayMouse = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hitMouse;

            if (Physics.Raycast(rayMouse, out hitMouse))
            {
                if (hitMouse.rigidbody != null)
                {
                    Vector3 dir = hitMouse.transform.position - transform.position;
                    hitMouse.rigidbody.AddForce(dir.normalized * physicsPower);

                    Debug.DrawRay(transform.position, dir, Color.red, Vector3.Distance(transform.position, hitMouse.transform.position));
                }
            }
        }
    }
}
