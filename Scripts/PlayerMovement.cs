using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CharacterMovement
{
    [Header("Rotation")]
    public Transform trnYAxisRotationReference;



    protected override void Awake()
    {
        base.Awake();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (trnDestination != null) // Auto movement -> Don't take input.
        {
            return;
        }

        // Input
        // Character move input
        inputAxisHor = Input.GetAxis("Horizontal");
        inputAxisVer = Input.GetAxis("Vertical");
        inputJump = Input.GetButtonDown("Jump");
        // Camera move input
        inputAxisCamX = Input.GetAxis("Mouse X");
        inputAxisCamY = Input.GetAxis("Mouse Y");
        inputAxisCamZ = Input.GetAxis("Mouse ScrollWheel");

        Quaternion moveDir = Quaternion.identity;
        Vector3 moveAxis = Vector3.zero;

        // Rotation
        if (inputAxisHor != 0f || inputAxisVer != 0f)
        {
            // inputAngle : Front 0, Back 180, Left -90, Right 90
            float inputAngle = Mathf.Atan2(inputAxisHor, inputAxisVer) * Mathf.Rad2Deg;

            moveDir = Quaternion.Euler(Vector3.up * trnYAxisRotationReference.eulerAngles.y);
            moveAxis = Vector3.right * inputAxisHor + Vector3.forward * inputAxisVer;

            // Character Rotation : Character and camera move in the apposite direction (in Y axis).
            // Example : 
            // 1. Input Right key -> inputAngle is 90
            // 2. Character moves Right -> Character rotates (inputAngle + Camera's current angle)

            // Actual moving
            // Using NavMeshAgent's Rotation function.
            _NavMeshAgent.SetDestination(transform.position + moveDir * moveAxis);
        }

        // Animation parameter
        _Animator.SetFloat("move", Mathf.Max(Mathf.Abs(inputAxisHor), Mathf.Abs(inputAxisVer)));
    }
}
