using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshHelper : MonoBehaviour
{
    public Transform target;
    
    // Start is called before the first frame update
    //void Start()
    //{
    //    GetComponent<NavMeshAgent>().SetDestination(target.position);
    //}

    // Update is called once per frame
    void Update()
    {
        GetComponent<NavMeshAgent>().SetDestination(target.position);
    }
}
