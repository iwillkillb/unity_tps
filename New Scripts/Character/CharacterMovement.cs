using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{
    // Components
    protected Animator _Animator;
    protected NavMeshAgent _NavMeshAgent;

    // Movement
    protected Quaternion moveDir;
    protected Vector3 moveAxis;

    [Header("Destination")]
    public Transform trnDestination;

    [Header("Staring mode")]
    public Transform trnStaringTarget;
    public float upperBodyAngle;

    // Input field
    public float inputAxisHor { get; set; }
    public float inputAxisVer { get; set; }
    public bool inputJump { get; set; }



    protected virtual void Awake()
    {
        // Components connecting
        _Animator = GetComponent<Animator>();
        _NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    protected virtual void FixedUpdate()
    {
        if (trnDestination != null)
        {
            _NavMeshAgent.SetDestination(trnDestination.position);
        }
        /*
        Vector3 mousePos = Input.mousePosition;
        // Left click
        if (Input.GetMouseButtonDown(0))
        {
            Ray rayMouse = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hitMouse;

            if (Physics.Raycast(rayMouse, out hitMouse))
            {
                // Click point
                if (trnCursor != null)
                {
                    trnCursor.position = hitMouse.point;
                }

                // Move
                _NavMeshAgent.SetDestination(hitMouse.point);
            }
        }*/
    }

    void SetAnimatorParameters()
    {
        // No Animator Component -> Disable

        if (_Animator == null)
        {
            return;
        }

        // Calculate difference of angle between upper and lower body.
        // Upper Body : Direction to target or Camera's angle.
        // Lower Body : This transform's rotation.
        // 0f : 0  (No difference)
        // 1f : 45 (Max difference)
        //float upperBodyAngle;

        // Animation parameter
        _Animator.SetFloat("move", Mathf.Max(Mathf.Abs(inputAxisHor), Mathf.Abs(inputAxisVer)));
    }

    float GetUpperBodyAngle(Transform trnStaringTarget)
    {
        // This calculates the angle(~180 ~ 180) from me to trnStaringTarget
        float result = 0f;

        Vector2 start = new Vector2(transform.position.x, transform.position.z);
        Vector2 end = new Vector2(trnStaringTarget.position.x, trnStaringTarget.position.z);
        Vector2 v2 = end - start;

        result = (Mathf.Atan2(v2.x, v2.y) * Mathf.Rad2Deg) - transform.eulerAngles.y;

        // Return Degree angle between Itself and Target. (-180 ~ 180)
        if (result < -180f)
        {
            result += 360f;
        }

        return result;
    }
}
