using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;


public class CollisionSystem : ComponentSystem
{
    private EntityQuery _collisonQuery;
    private Collider[] _results = new Collider[50];

    private EntityManager _entityManager;

    protected override void OnCreate()
    {
        //_collisonQuery = GetEntityQuery(ComponentType.ReadOnly<ActorColliderData>(), ComponentType.ReadOnly<Transform>());
        var query = new EntityQueryDesc
        {
            None = new ComponentType[] { },
            All = new ComponentType[] { typeof(ActorColliderData), ComponentType.ReadOnly<Transform>() }
        };
        _collisonQuery = GetEntityQuery(query);

        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    protected override void OnUpdate()
    {
        Entities.With(_collisonQuery).ForEach(
            (Entity entity, Transform transform, ref ActorColliderData colliderData) =>
            {
                if (transform == null)//баг с мечом
                    return;
                
                var gameObject = transform.gameObject;
                float3 position = gameObject.transform.position;
                Quaternion rotation = gameObject.transform.rotation;

                int size = 0;

                var abilityColision = gameObject.GetComponent<ICollisionAbility>();
                abilityColision?.Collisions?.Clear();
                Array.Clear(_results,0, _results.Length);

                switch (colliderData.ColliderType)
                {
                    case ColliderType.Sphere:
                        size = Physics.OverlapSphereNonAlloc(colliderData.SphereCenter + position, colliderData.SphereRadius, _results,colliderData.layerMask);
                        break;
                    case ColliderType.Capsule:
                        var point1 = colliderData.CapsuleStart + position;
                        var point2 = colliderData.CapsuleEnd + position;
                        var center = (point1 + point2) / 2f;

                        point1 = (float3)(rotation * (point1 - center)) + center;
                        point2 = (float3)(rotation * (point2 - center)) + center;

                        size = Physics.OverlapCapsuleNonAlloc(point1, point2, colliderData.CapsuleRadius, _results,colliderData.layerMask);
                        break;
                    case ColliderType.Box:
                        size = Physics.OverlapBoxNonAlloc(colliderData.BoxCenter + position,
                        colliderData.BoxHalfExtens, _results, colliderData.BoxOrientation*rotation,colliderData.layerMask);
                        break;
                }

                if (size > 0)
                {
                    abilityColision.Collisions = ConvertArrayToList(_results);
                    //Debug.Log(_results[0]);
                    abilityColision.Execute();
                    if (abilityColision.DestroyAfterCollision)
                    {
                        _entityManager.DestroyEntity(entity);
                        UnityEngine.Object.Destroy(transform.gameObject);
                    } 
                }
            }
        );
    }

    private List<Collider> ConvertArrayToList(Collider[] array)
    {
        List<Collider> nl  = new ();
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] != null)
            {
                nl.Add(array[i]);
            }
        }

        return nl;
    }
}
