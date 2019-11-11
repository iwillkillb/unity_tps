using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementCC : MonoBehaviour
{
    // Components
    Animator _Animator;
    CharacterController _CharacterController;

    // Input field
    public float inputAxisHor { get; set; }
    public float inputAxisVer { get; set; }
    public float inputAxisCamX { get; set; }
    public float inputAxisCamY { get; set; }
    public float inputAxisCamZ { get; set; }
    public bool inputJump { get; set; }

    [Header("Move")]
    public float moveSpeed = 7f;
    public float rotateSpeed = 10f;
    Vector3 moveAxis;
    float currentClimbAngle;

    [Header("Rotation")]
    public bool useNavMeshAgentRotation = true;
    public Transform trnYAxisRotationReference;

    [Header("Jump")]
    public float vVelocity = 0f;
    public float gravity = 9.81f;
    public float jumpVelocity = 10f;

    // Start is called before the first frame update
    void Awake()
    {
        // Components connecting
        _Animator = GetComponent<Animator>();
        _CharacterController = GetComponent<CharacterController>();
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

        if (_CharacterController.isGrounded)
        {
            vVelocity = -gravity * Time.deltaTime;
            if (inputJump)
            {
                if (!_Animator.IsInTransition(0))
                {
                    vVelocity = jumpVelocity;
                }
            }
        }
        else
        {
            vVelocity -= gravity * Time.deltaTime;
        }

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

            // Crispy rotation
            //transform.eulerAngles = Vector3.up * (inputAngle + trnYAxisRotationReference.eulerAngles.y);

            // Fuzzy rotation
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.Euler(0f, inputAngle + trnYAxisRotationReference.eulerAngles.y, 0f),
                rotateSpeed * Time.deltaTime);
        }

        // Actual moving
        _CharacterController.Move((Vector3.up * vVelocity + moveDir * moveAxis * moveSpeed) * Time.deltaTime);

        // Animation parameter
        _Animator.SetBool("isGround", _CharacterController.isGrounded);
        _Animator.SetFloat("move", Mathf.Max(Mathf.Abs(inputAxisHor), Mathf.Abs(inputAxisVer)));
    }
}
