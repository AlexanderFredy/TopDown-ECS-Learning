using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DefenceFromShootBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField]
    private float visionRadius;

    private WeaponAbility currentTarget;
    private Animator _anim;
    private NavMeshAgent _nma;

    public bool InProgress { get; set; }

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _nma = GetComponent<NavMeshAgent>();
    }

    public void Behave()
    {
        _nma.enabled = false;
        //transform.LookAt(currentTarget.transform.position,Vector3.up);
        transform.LookAt(new Vector3(currentTarget.transform.position.x, transform.position.y, currentTarget.transform.position.z));

        _anim.SetBool("shield", true);
        //Debug.Log("Defence");
    }

    public float Evaluate()
    {
        Collider[] avalableTargets = new Collider[50];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, visionRadius, avalableTargets, 1 << 7);

        for (int i = 0; i < numColliders; i++)
        {
            var bul = avalableTargets[i].transform.GetComponent<WeaponAbility>();
            if (bul != null && bul.IsBullet && bul.Owner != null
                && Vector3.Distance(transform.position,bul.transform.position) < 3f)
            {
                currentTarget = bul;
                return 1f;
            }
        }

        return 0f;
    }
}
