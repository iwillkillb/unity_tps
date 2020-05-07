using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementCC : MonoBehaviour
{
    // Components
    Animator _Animator;
    CharacterController _CharacterController;

    public Interactable focus;

    // Input field
    public float inputAxisHor { get; set; }
    public float inputAxisVer { get; set; }
    public bool inputJump { get; set; }

    [Header("Rotation")]
    public float rotationSlerp = 5f;        // This is used by smooth rotation If you don't use NavMeshAgent's rotation.
    public bool isStaringFront = true;      // True  : Character stares Camera's direction | False : The character looks in the direction in which it moves.

    // Actual Rotation
    float inputAngle;   // Get angle by input.
    Quaternion preRot;  // Get rotation value in Previous frame.
    Quaternion newRot;  // Set new direction rotation value.
    Quaternion moveDir;

    // Actual Moving
    Vector3 moveAxis;

    [Header("Move")]
    public float moveSpeed = 7f;
    public float slopeForce = 5f;
    float currentSlopeForce;

    [Header("Jump")]
    public float gravity = 9.81f;
    public float jumpForce = 10f;
    float vVelocity = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        // Components connecting
        _Animator = GetComponent<Animator>();
        _CharacterController = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        // Initialization
        moveAxis = Vector3.zero;
        moveDir = Quaternion.identity;

        // Rotation backup
        preRot = transform.rotation;
        newRot = transform.rotation;



        // Input
        inputAxisHor = Input.GetAxis("Horizontal");
        inputAxisVer = Input.GetAxis("Vertical");
        inputJump = Input.GetButtonDown("Jump");



        // Ground check and gravity
        if (_CharacterController.isGrounded)
        {
            vVelocity = -gravity * Time.deltaTime;
            if (inputJump)
            {
                /*
                if (!_Animator.IsInTransition(0))
                {
                    vVelocity = jumpForce;
                }*/

                // Jump -> Don't care slope
                currentSlopeForce = 0f;
                vVelocity = jumpForce;
            }
            else
            {
                // Reset slope force
                if (currentSlopeForce != slopeForce)
                {
                    currentSlopeForce = slopeForce;
                }
            }
        }
        else
        {
            vVelocity -= gravity * Time.deltaTime;
        }


        
        // When character moving...
        if (inputAxisHor != 0f || inputAxisVer != 0f)
        {
            RemoveFocus();

            // Character Rotation : Character and camera move in the apposite direction (in Y axis).
            // Example : 
            // 1. Input Right key -> inputAngle is 90
            // 2. Character moves Right -> Character rotates (inputAngle + Camera's current angle)

            // inputAngle : Front 0, Back 180, Left -90, Right 90
            inputAngle = Mathf.Atan2(inputAxisHor, inputAxisVer) * Mathf.Rad2Deg;

            // Get trnRotationReference's y rotation.
            moveDir = Quaternion.Euler(Vector3.up * Camera.main.transform.eulerAngles.y);

            // Use Input data
            moveAxis = Vector3.right * inputAxisHor + Vector3.forward * inputAxisVer;

            // No stare -> Rotate only moving
            if (!isStaringFront)
            {
                newRot = Quaternion.Euler(0f, inputAngle + Camera.main.transform.eulerAngles.y, 0f);
            }
        }

        // Stare
        if (isStaringFront)
        {
            newRot = Quaternion.Euler(Vector3.up * Camera.main.transform.eulerAngles.y);
        }

        // Actual Rotation
        transform.rotation = Quaternion.Slerp(preRot, newRot, rotationSlerp * Time.smoothDeltaTime);

        // Slope Check -> Force to down -> Contact on slope -> No Bouncing
        if (OnSlope())
        {
            vVelocity -= _CharacterController.height * 0.5f * currentSlopeForce;
        }

        // Actual Moving
        _CharacterController.Move((Vector3.up * vVelocity + moveDir * moveAxis * moveSpeed) * Time.smoothDeltaTime);




        // Right click -> Focus on Object.
        if (Input.GetMouseButtonDown(1))
        {
            Ray rayMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitMouse;

            if (Physics.Raycast(rayMouse, out hitMouse))
            {
                // Move to Click point
                Interactable interactable = hitMouse.collider.transform.GetComponent<Interactable>();
                if (interactable != null)
                {
                    SetFocus(interactable);
                }
            }
        }

    }



    bool OnSlope()
    {
        if (!_CharacterController.isGrounded)
        {
            return false;
        }

        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f))
        {
            if(hit.normal != Vector3.up)
            {
                return true;
            }
        }

        return false;
    }



    /*
    void LookAtPoint(Vector3 point)
    {
        Vector3 direction = (point - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }*/

    void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        {
            if (focus != null)
            {
                focus.OnDefocused();
            }
            focus = newFocus;

        }
        newFocus.OnFocused(transform);
    }

    void RemoveFocus()
    {
        if (focus != null)
        {
            focus.OnDefocused();
        }
        focus = null;
    }
}
