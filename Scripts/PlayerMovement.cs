using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class takes player's keyboard input.

public class PlayerMovement : CharacterMovement
{
    [Header("Movement")]
    public bool isUsingNavMovement = false;

    [Header("Rotation")]
    public float rotationSlerp = 5f;        // This is used by smooth rotation If you don't use NavMeshAgent's rotation.
    public Transform trnRotationReference;  // Transform with rotation value referenced.
    public bool isStaringFront = true;      // True  : Character stares Camera's direction | False : The character looks in the direction in which it moves.

    float inputAngle;   // Get angle by input.
    Quaternion preRot;  // Rotation value in Previous frame.


    protected override void Awake()
    {
        base.Awake();

        // Player don't use NavMeshAgent's rotation.
        _NavMeshAgent.updateRotation = false;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // If you have destination already, you can't control character.
        if (trnDestination != null)
        {
            return;
        }

        // Input
        // Character move input

        if (Input.GetKey(KeyCode.RightShift))
        {
            inputAxisHor = Input.GetAxis("Horizontal") * 0.5f;
            inputAxisVer = Input.GetAxis("Vertical") * 0.5f;
        }
        else
        {
            inputAxisHor = Input.GetAxis("Horizontal");
            inputAxisVer = Input.GetAxis("Vertical");
        }

        // Get Rotation value
        preRot = transform.rotation;

        // Stare Camera's View
        if (isStaringFront)
        {
            transform.rotation = Quaternion.Slerp(preRot, Camera.main.transform.rotation, 10f * Time.deltaTime);
        }

        // Rotation
        if (inputAxisHor != 0f || inputAxisVer != 0f)
        {
            // inputAngle : Front 0, Back 180, Left -90, Right 90
            inputAngle = Mathf.Atan2(inputAxisHor, inputAxisVer) * Mathf.Rad2Deg;

            // Get trnRotationReference's y rotation.
            moveDir = Quaternion.Euler(Vector3.up * trnRotationReference.eulerAngles.y);

            // Use Input data
            moveAxis = Vector3.right * inputAxisHor + Vector3.forward * inputAxisVer;

            // Character Rotation : Character and camera move in the apposite direction (in Y axis).
            // If NavMeshAgent's updateRotation is false, calculate the rotation directly.
            // Example : 
            // 1. Input Right key -> inputAngle is 90
            // 2. Character moves Right -> Character rotates (inputAngle + Camera's current angle)
            if (!isStaringFront)
            {
                transform.rotation = Quaternion.Slerp(
                    preRot,
                    Quaternion.Euler(0f, inputAngle + trnRotationReference.eulerAngles.y, 0f),
                    rotationSlerp * Time.smoothDeltaTime);
            }

            // Actual moving
            // Using NavMeshAgent's Rotation function.
            if (isUsingNavMovement)
            {
                _NavMeshAgent.SetDestination(transform.position + moveDir * moveAxis);
            }
            else
            {
                transform.position += moveDir * moveAxis * Time.smoothDeltaTime * _NavMeshAgent.speed;
            }
        }
    }
}
