using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIDrift : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform target;
    private NavMeshAgent navMeshAgent;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        navMeshAgent.SetDestination(target.transform.position);
    }
}
