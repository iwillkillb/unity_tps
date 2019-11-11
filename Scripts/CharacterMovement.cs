using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{
    // Components
    protected Animator _Animator;
    protected NavMeshAgent _NavMeshAgent;

    // Input field
    public float inputAxisHor { get; set; }
    public float inputAxisVer { get; set; }
    public float inputAxisCamX { get; set; }
    public float inputAxisCamY { get; set; }
    public float inputAxisCamZ { get; set; }
    public bool inputJump { get; set; }

    [Header("Auto movement")]
    public Transform trnDestination;



    // Start is called before the first frame update
    protected virtual void Awake()
    {
        // Components connecting
        _Animator = GetComponent<Animator>();
        _NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        if (trnDestination != null)
        {
            _NavMeshAgent.SetDestination(trnDestination.position);

            // Animator Parameter
            _Animator.SetFloat("move", _NavMeshAgent.velocity.magnitude / _NavMeshAgent.speed);
        }
    }
}
