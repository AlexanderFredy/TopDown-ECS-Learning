using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Zenject;

public class WeaponAbility : CollisionAbility, ICollisionAbility, IWeapon
{
    [field:SerializeField]
    public int Damage { get; set; }
    
    [field:SerializeField]
    public float DamageInterval { get; set; }
    
    [field:SerializeField]
    public Vector3 HandlerLocalPos { get; set; }
    [field:SerializeField]
    public Vector3 HandlerLocalRot { get; set; }
    
    [field:SerializeField]
    public GameObject Owner { get; set; }
    
    public float _destroyTimeInFlight;
    public float _flightSpeed;
    public bool IsBullet;

    [SerializeField]
    private AK.Wwise.Event strikeEvent;

    private float LastDamageTime = 0f;
    
    public void MakeDamage(CharacterHealth enemyHealth)
    {
        enemyHealth.ApplyDamage(Damage);
    }

    new public void Execute()
    {
        if (Time.time - LastDamageTime > DamageInterval)
            LastDamageTime = Time.time;
        else
            return;
        
        // if (TryGetComponent(out Rigidbody rb))
        //     rb.isKinematic = false;
        
        //foreach (var target in Collisions)
        //{
            if (Owner != null)//ничейное оружие не наносит урон никому
            {
                var target = Collisions[0];
                var enemyHealth = target.gameObject.transform.root.GetComponentInChildren<CharacterHealth>();
                if (enemyHealth != null) //если нечему наносить урон
                {
                    MakeDamage(enemyHealth);
                    
                    //остаётся в теле, в которое попало
                    // transform.parent = target.transform;
                    // transform.position = target.transform.position;
                }

                strikeEvent.Post(gameObject); strikeEvent.Stop(gameObject);
            }
            else
                return;
            
            //ЛЕТАЮЩЕЕ ОРУЖИЕ НУЖНО ДОРАБОТАТЬ
            // this.ClearCollisionLayerAndOwner();
            //
            //
            // var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            // if (entityManager.HasComponent<BulletTag>(MyEntity))
            // {
            //     BulletTag data = entityManager.GetComponentData<BulletTag>(MyEntity);
            //     data.DontMove = true;
            //     entityManager.SetComponentData(MyEntity, data);
            // };
            
        //}
    }
    
    protected override void AddDataToComponentsAfterInstansing()
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        entityManager.AddComponentData(MyEntity, new BulletTag
        {
            Speed = _flightSpeed, 
            TimeDestroy = _destroyTimeInFlight,
            DontDestroy =  !DestroyAfterCollision, 
            DontMove = !IsBullet
        });
    }
}

public class WeaponFactory: PlaceholderFactory<int, float, WeaponAbility>
{

}

public struct BulletTag: IComponentData
{
    public float TimeDestroy;
    public float Speed;
    public bool DontDestroy;
    public bool DontMove;
}