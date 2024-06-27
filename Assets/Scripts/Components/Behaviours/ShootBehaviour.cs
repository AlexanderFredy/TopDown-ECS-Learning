using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System.Threading.Tasks;
using UnityEngine.AI;
using Zenject;

public class ShootBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField]
    private float attackDelay;
    [SerializeField]
    private float visionRadius;
    [SerializeField]
    private WeaponAbility bulletPrefab;
    public int BulletCount;
    public bool bulletDontDestroyAfterCollision;
    public bool bulletDontMoveAfterCollision;

    private CharacterHealth currentTarget;
    private Animator _anim;
    private BehaviourManager _bhmngr;
    
    [Inject]
    private GameManager _gameManager;

    public bool InProgress { get; set; }


    private void Start()
    {
        _anim = GetComponent<Animator>();
        _bhmngr = GetComponent<BehaviourManager>();
    }

    public void Behave()
    {
        if (currentTarget == null)
            return;

        _bhmngr.DontInterrupt = true;

        Vector3 curTargPos = currentTarget.transform.position;
        transform.LookAt(curTargPos);

        _anim.SetTrigger("Attack");      
        //await SpellCast();

        Utils.DrawCircle(transform.position, visionRadius, Color.blue);

        _bhmngr.DontInterrupt = false;
    }

    public float Evaluate()
    {
        if (InProgress)
            return 1f;

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

    private void SpellCast()
    {
        var angle = 360f/ BulletCount;

        var t = transform;
        Vector3 starPosition = t.position;
        starPosition.y += 1f;

        for (int i = 0; i < BulletCount; i++)
        {
            Quaternion newRotation = Quaternion.AngleAxis(angle*i, Vector3.up);
            var bul = Instantiate(bulletPrefab,starPosition, newRotation);
            bul.SetCollisionLayerAndOwner(this.gameObject);
            bul.DestroyAfterCollision = !bulletDontDestroyAfterCollision;
            bul.DontMoveAfterCollision = bulletDontMoveAfterCollision;
            bul.IsBullet = true;
        }
    }
    
    public void ClearTarget()
    {
        currentTarget = null;
    }

}
