using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    public Transform target;

    NavMeshAgent _NavMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        _NavMeshAgent = GetComponent<NavMeshAgent>();
        Debug.Log(transform.position + " is Position.");
    }

    // Update is called once per frame
    void Update()
    {
        _NavMeshAgent.SetDestination(target.position);
    }
}
