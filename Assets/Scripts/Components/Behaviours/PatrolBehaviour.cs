using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField]
    private float visionRadius;
    [SerializeField]
    private Transform _patrolTarget;

    private NavMeshAgent _nma;
    private Animator _anim;
    private float _baseSpeed;

    public bool InProgress { get; set; }

    private void Start()
    {
        _nma = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        _baseSpeed = _nma.speed;
        _patrolTarget = GameObject.Find("PatrolTarget").transform;
    }

    public void Behave()
    {
        _nma.enabled = true;

        if (Vector3.Distance(transform.position, _patrolTarget.position) > 0.5f)
        {
            _nma.speed = _baseSpeed;
            _nma.SetDestination(_patrolTarget.position);
        }
        else
            _patrolTarget.position = Utils.GetPositionInArea();

        _anim.SetBool("attack", false);
        _anim.SetBool("walk", true);
        _anim.SetBool("fight", false);
        _anim.SetBool("shield", false);
    }

    public float Evaluate()
    { 
        return 0.5f;
    }

}
