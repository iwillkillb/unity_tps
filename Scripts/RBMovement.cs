using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RBMovement : MonoBehaviour
{
    // Components
    public Animator _Animator;
    Rigidbody _Rigidbody;
    CapsuleCollider _CapsuleCollider;

    // Input field
    public float inputAxisHor { get; set; }
    public float inputAxisVer { get; set; }
    public bool inputJump { get; set; }

    [Header("Move")]
    public float moveSpeed = 7f;
    public float acceleration = 20f;
    public float slopeForce = 5f;
    float inputAngle;   // Get angle by input.

    [Header("Rotation")]
    public float rotationSlerp = 5f;        // This is used by smooth rotation If you don't use NavMeshAgent's rotation.
    public bool isStaringFront = true;
    // True  : Character stares Camera's direction.
    // False : The character looks in the direction in which it moves.

    [Header("Jump")]
    public float jumpForce = 10f;

    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashTime = 0.5f;
    float currentDashTime = 0f;

    [Header("Terrain Check")]
    public LayerMask terrainLayer;
    bool isGrounded;

    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.1f;

    public Transform slopeCheckPoint;
    public float slopeCheckRange = 1f;



    // Start is called before the first frame update
    void Awake()
    {
        // Components connecting
        _Rigidbody = GetComponent<Rigidbody>();
        _CapsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        // Ground Check
        isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, terrainLayer);

        // Jump
        if (inputJump && isGrounded)
        {
            _Rigidbody.AddRelativeForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }

        // Input
        inputAxisHor = Input.GetAxis("Horizontal");
        inputAxisVer = Input.GetAxis("Vertical");
        inputJump = Input.GetButtonDown("Jump");

        //SetVerticalVelocity();
        Movement();
        Rotation(isStaringFront);
        Dash();

        SetAnimationParameter();
    }

    Vector3 GetGroundNormal()
    {
        RaycastHit slopeHit;
        Vector3 groundNormal = Vector3.up;

        // Slope Check
        if (Physics.Raycast(slopeCheckPoint.position, -transform.up, out slopeHit, slopeCheckRange, terrainLayer))
        {
            groundNormal = slopeHit.normal;
        }

        return groundNormal;
    }

    void Movement()
    {
        // Initialization actual moving values.
        Vector3 moveAxis = Vector3.zero;
        Quaternion moveDir = Quaternion.identity;
        Quaternion groundQuaternion = Quaternion.FromToRotation(transform.up, GetGroundNormal());
        Vector3 resultVelocity;

        if (inputAxisHor != 0f || inputAxisVer != 0f)
        {
            // Character Rotation : Character and camera move in the apposite direction (in Y axis).
            // Example : 
            // 1. Input Right key -> inputAngle is 90
            // 2. Character moves Right -> Character rotates (inputAngle + Camera's current angle)

            // inputAngle : Front 0, Back 180, Left -90, Right 90
            inputAngle = Mathf.Atan2(inputAxisHor, inputAxisVer) * Mathf.Rad2Deg;

            // Get trnRotationReference's y rotation.
            moveDir = Quaternion.Euler(Vector3.up * Camera.main.transform.eulerAngles.y);

            // Use Input data
            moveAxis = (Vector3.right * inputAxisHor + Vector3.forward * inputAxisVer).normalized;

            // Wall Check
            RaycastHit hitBody;
            Vector3 capsuleCastPoint1 = transform.position + _CapsuleCollider.center - (Vector3.up * (_CapsuleCollider.height * 0.5f - _CapsuleCollider.radius - 0.1f));
            Vector3 capsuleCastPoint2 = transform.position + _CapsuleCollider.center + (Vector3.up * (_CapsuleCollider.height * 0.5f - _CapsuleCollider.radius - 0.1f));
            if (Physics.CapsuleCast(capsuleCastPoint1, capsuleCastPoint2, _CapsuleCollider.radius, groundQuaternion * moveDir * moveAxis, out hitBody, 0.1f, terrainLayer))
            {
                // WWWWW                 WWWWW
                // WWW                     WWW
                // W                         W
                //   C                     C
                //   |                     |
                //  -30 -> rotate 60       30 -> rotate -60
                // Angle perpendicular to the wall
                float angleToWall = Vector3.SignedAngle(moveDir * moveAxis, -hitBody.normal, Vector3.up);

                // New direction setting : Collision with wall -> Move side of wall(Perpendicular to the wall)
                if (angleToWall > 0f)
                    moveDir = Quaternion.Euler(Vector3.up * (Camera.main.transform.eulerAngles.y + (-90 + angleToWall)));
                else
                    moveDir = Quaternion.Euler(Vector3.up * (Camera.main.transform.eulerAngles.y + (90 + angleToWall)));
            }
        }
        // Setting velocity
        resultVelocity = groundQuaternion * (moveDir * moveAxis) * moveSpeed;

        // Actual Moving
        _Rigidbody.velocity = isGrounded ? resultVelocity : new Vector3(resultVelocity.x, _Rigidbody.velocity.y, resultVelocity.z);

        /*
        if (_Rigidbody.velocity.magnitude < moveSpeed)
            _Rigidbody.AddForce(resultVelocity * acceleration, ForceMode.Acceleration);
        // Break
        if (inputAxisHor == 0f && inputAxisVer == 0f)
        {
            Vector3 breakVelocity = _Rigidbody.velocity;
            breakVelocity.x *= 0.9f;
            breakVelocity.z *= 0.9f;
            _Rigidbody.velocity = breakVelocity;
        }
        */
    }

    void Rotation(bool isStaringFront)
    {
        // Rotation backup
        Quaternion preRot = transform.rotation;  // Get rotation value in Previous frame.
        Quaternion newRot = transform.rotation;  // Set new direction rotation value.

        // Stare
        if (isStaringFront)
        {
            newRot = Quaternion.Euler(Vector3.up * Camera.main.transform.eulerAngles.y);
        }
        // No stare -> Rotate only moving
        else if (inputAxisHor != 0f || inputAxisVer != 0f)
        {
            newRot = Quaternion.Euler(Vector3.up * (inputAngle + Camera.main.transform.eulerAngles.y));
        }

        // Actual Rotation
        transform.rotation = Quaternion.Slerp(preRot, newRot, rotationSlerp * Time.deltaTime);
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            currentDashTime = dashTime;
        }

        if (currentDashTime > 0f)
        {
            currentDashTime -= Time.deltaTime;

            Rotation(true);

            // Dash to camera's forward
            //_CharacterController.Move(Quaternion.Euler(Vector3.up * Camera.main.transform.eulerAngles.y) * Vector3.forward * (dashSpeed * currentDashTime / dashTime) * Time.deltaTime);
            _Rigidbody.velocity = Quaternion.Euler(Vector3.up * Camera.main.transform.eulerAngles.y) * Vector3.forward * (dashSpeed * currentDashTime / dashTime);
        }
    }

    void SetAnimationParameter()
    {
        if (_Animator == null)
        {
            return;
        }

        if (isStaringFront)
        {
            _Animator.SetFloat("move", inputAxisVer);
            _Animator.SetFloat("direction", inputAxisHor);
        }
        else
        {
            _Animator.SetFloat("move", Mathf.Max(Mathf.Abs(inputAxisHor), Mathf.Abs(inputAxisVer)));
        }

        _Animator.SetBool("isGrounded", isGrounded);
    }



    /*
    void LookAtPoint(Vector3 point)
    {
        Vector3 direction = (point - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }*/
}
