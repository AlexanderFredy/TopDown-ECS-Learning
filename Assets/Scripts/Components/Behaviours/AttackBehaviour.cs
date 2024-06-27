using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class AttackBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField]
    private float attackRadius;
    [SerializeField]
    private float attackDelay;
    [SerializeField]
    private float speedMultiplyer;
    [SerializeField]
    private float visionRadius;
    [SerializeField] 
    private AK.Wwise.Event strikeEvent;

    private CharacterHealth currentTarget;
    private Animator _anim;
    private NavMeshAgent _nma;
    private BehaviourManager _bhmngr;
    private float _lastAttackTime;
    private float _baseSpeed;

    [Inject]
    private GameManager _gameManager;

    public bool InProgress { get; set; }

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _nma = GetComponent<NavMeshAgent>();
        _bhmngr = GetComponent<BehaviourManager>();
        _baseSpeed = _nma.speed;
    }

    public void Behave()
    {
        if (currentTarget == null)
            return;

        Vector3 curTargPos = currentTarget.transform.position;

        if (Vector3.Distance(transform.position, curTargPos) < attackRadius)//атакуем
        {
            _nma.enabled = false;
            transform.LookAt(curTargPos);

            if (Time.time - _lastAttackTime > attackDelay)
            {
                _anim.SetBool("fight", true);
                _lastAttackTime = Time.time;
                Invoke("SwordSound",0.3f);
            } else
            {
                _anim.SetBool("fight", false);
            }
            _anim.SetBool("walk", false);
        } else//преследуем цель
        {
            _nma.enabled = true;
            _nma.speed = _baseSpeed * speedMultiplyer;
            _nma.SetDestination(curTargPos);

            _anim.SetBool("attack", true);
            _anim.SetBool("walk", true);
            _anim.SetBool("fight", false);
        }

        Utils.DrawCircle(transform.position, visionRadius, Color.blue);
        Utils.DrawCircle(transform.position, attackRadius, Color.red);
    }

    private void SwordSound() => strikeEvent.Post(gameObject);

    public float Evaluate()
    {
        if (!_bhmngr.IhaveWeapon)
            return 0;     
        
        if (currentTarget != null && !_gameManager.PlayerIsDead 
            && Vector3.Distance(transform.position, currentTarget.transform.position) < visionRadius)
        {
            return 1f;
        }
        else
        {
            Collider[] avalableTargets = new Collider[50];
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, visionRadius, avalableTargets, 1 << 6);

            for (int i = 0; i < numColliders; i++)
            {
                var charH = avalableTargets[i].transform.GetComponent<CharacterHealth>();
                var invisAbility = avalableTargets[i].transform.GetComponent<InvisibleAbility>();

                if (charH != null && !invisAbility.invisNow && !_gameManager.PlayerIsDead)
                {
                    currentTarget = charH;
                    return 1f;
                }
            }
            currentTarget = null;
            
            return 0f;
        }
    }

    public void ClearTarget()
    {
        currentTarget = null;
    }

}
