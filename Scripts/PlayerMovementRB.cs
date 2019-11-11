using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementRB : MonoBehaviour
{
    // Components
    Animator _Animator;
    AnimatorStateInfo animStateBaseLayer;
    Rigidbody _Rigidbody;

    // Collider
    CapsuleCollider _CapsuleCollider;
    float colOrgHeight;
    float colOrgRadius;
    Vector3 colOrgCenter;

    // Input field
    public float inputAxisHor { get; set; }
    public float inputAxisVer { get; set; }
    public float inputAxisCamX { get; set; }
    public float inputAxisCamY { get; set; }
    public float inputAxisCamZ { get; set; }
    public bool inputJump { get; set; }

    // Ground check
    [Header("Ground check")]
    bool isGround;
    public LayerMask terrainLayerMask;

    [Header("Move")]
    public float moveSpeed = 7.0f;
    public float rotateSpeed = 2.0f;
    public float maxClimbAngle = 45f;
    Vector3 moveAxis;
    float currentClimbAngle;

    [Header("Rotation")]
    public bool useNavMeshAgentRotation = true;
    public Transform trnYAxisRotationReference;

    [Header("Jump")]
    public float jumpVelocity = 5.0f;



    void Awake()
    {
        // Components connecting
        _Animator = GetComponent<Animator>();
        animStateBaseLayer = _Animator.GetCurrentAnimatorStateInfo(0);
        _CapsuleCollider = GetComponent<CapsuleCollider>();
        _Rigidbody = GetComponent<Rigidbody>();

        // Backup collider's data
        colOrgHeight = _CapsuleCollider.height;
        colOrgRadius = _CapsuleCollider.radius;
        colOrgCenter = _CapsuleCollider.center;
    }

    void FixedUpdate()
    {
        // Input
        // Character move input
        inputAxisHor = Input.GetAxis("Horizontal");
        inputAxisVer = Input.GetAxis("Vertical");
        inputJump = Input.GetButtonDown("Jump");
        // Camera move input
        inputAxisCamX = Input.GetAxis("Mouse X");
        inputAxisCamY = Input.GetAxis("Mouse Y");
        inputAxisCamZ = Input.GetAxis("Mouse ScrollWheel");

        // Checking ground
        RaycastHit hitGround = new RaycastHit();
        isGround = Physics.SphereCast(
            transform.TransformPoint(colOrgCenter),
            colOrgRadius,
            Vector3.down,
            out hitGround,
            colOrgHeight * 0.5f - colOrgRadius,
            terrainLayerMask
        );

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

            transform.eulerAngles = Vector3.up * (inputAngle + trnYAxisRotationReference.eulerAngles.y);

            // Don't stuck when aerial.
            RaycastHit hitWall;
            if (_Rigidbody.SweepTest((moveDir * moveAxis).normalized, out hitWall, colOrgRadius))
            {
                // Wall's angle is more 45...
                if (!isGround && hitWall.normal.y < 0.7f)
                {
                    moveAxis = Vector3.zero;
                }
                Debug.Log("normal : " + hitWall.normal + ", point : " + hitWall.point);
            }

            // Actual moving
            transform.position += moveDir * moveAxis * moveSpeed * Time.smoothDeltaTime;
        }


        // Jump
        if (isGround)
        {
            if (inputJump)
            {
                if (!_Animator.IsInTransition(0))
                {
                    //_Animator.CrossFade("Jump", 0.1f);
                    Jump();
                }
            }
        }

        // Animation parameter
        _Animator.SetBool("isGround", isGround);
        _Animator.SetFloat("move", Mathf.Max(Mathf.Abs(inputAxisHor), Mathf.Abs(inputAxisVer)));
    }


    // ------------------------------------------------------------------------------------------------------------------
    // -------------------------------------------------Animation events-------------------------------------------------
    // ------------------------------------------------------------------------------------------------------------------

    public void Jump()
    {
        //_RigidBody.velocity = new Vector3(_RigidBody.velocity.x, jumpVelocity, _RigidBody.velocity.z);
        _Rigidbody.AddForce(Vector3.up * jumpVelocity, ForceMode.VelocityChange);
    }
}
