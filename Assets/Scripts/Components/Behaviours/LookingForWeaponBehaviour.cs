using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LookingForWeaponBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField]
    private float visionRadius;
    [SerializeField]
    private float takeWeaponRadius;
    [SerializeField]
    private float speedMultiplyer;

    private BehaviourManager _bhmngr;
    private WeaponAbility _weaponTarget;
    private NavMeshAgent _nma;
    private Animator _anim;
    private float _baseSpeed;

    public bool InProgress { get; set; }

    private void Start()
    {
        _nma = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        _bhmngr = GetComponent<BehaviourManager>();
        _baseSpeed = _nma.speed;
    }

    public void Behave()
    {
        _nma.enabled = true;

        if (_weaponTarget != null)
        {
            Transform weapTrans = _weaponTarget.transform;
            if (Vector3.Distance(transform.position, weapTrans.position) < takeWeaponRadius)
            {
                _weaponTarget.SetCollisionLayerAndOwner(this.gameObject);
                weapTrans.parent = _bhmngr.partOfBodyForWeaponPrefab;

                if (_weaponTarget.gameObject.TryGetComponent(out Rigidbody rb))
                {
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }

                // if (_weaponTarget.CompareTag("Sword"))
                // {
                //     weapTrans.localPosition = new Vector3(0.06f, 0.01f, 0.04f);
                //     weapTrans.localEulerAngles = new Vector3(-179f, -180f, 88.5f);
                // }
                // else if (_weaponTarget.CompareTag("Spire"))
                // {
                //     weapTrans.localPosition = new Vector3(-0.03f, 0.0f, 0.05f);
                //     weapTrans.localEulerAngles = new Vector3(3.4f, -109.6f, 44.2f);
                // }
                
                weapTrans.localPosition = _weaponTarget.HandlerLocalPos;
                weapTrans.localEulerAngles = _weaponTarget.HandlerLocalRot;

                _bhmngr.IhaveWeapon = true;
                _weaponTarget = null;
            }
            else if (Vector3.Distance(transform.position, weapTrans.position) < visionRadius+2f)
            {
                _nma.speed = _baseSpeed * speedMultiplyer;
                _nma.SetDestination(weapTrans.position);
            }
            else
            {
                _weaponTarget = null;
            }
        }

        _anim.SetBool("attack", false);
        _anim.SetBool("walk", true);
        _anim.SetBool("fight", false);
        _anim.SetBool("shield", false);

        Utils.DrawCircle(transform.position, visionRadius, Color.blue);
        Utils.DrawCircle(transform.position, takeWeaponRadius, Color.red);
    }

    public float Evaluate()
    {
        if (_bhmngr.IhaveWeapon)
            return 0f;

        Collider[] avalableTargets = new Collider[50];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, visionRadius, avalableTargets, 1 << 10);

        for (int i = 0; i < numColliders; i++)
        {
            var weapAbil = avalableTargets[i].transform.GetComponent<WeaponAbility>();
            if (weapAbil != null && weapAbil.Owner == null)
            {
                _weaponTarget = weapAbil;
                return 1f;
            }
        }

        return 0f;

    }
}
