using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class BulletBehaviourSystem : ComponentSystem
{
    private EntityQuery _bulletQuery;
    private EntityManager _entityManager;

    protected override void OnCreate()
    {
        //_bulletQuery = GetEntityQuery(ComponentType.ReadOnly<BulletTag>(), ComponentType.ReadOnly<Transform>());

        var query = new EntityQueryDesc
        {
            None = new ComponentType[] { },
            All = new ComponentType[] { typeof(BulletTag), ComponentType.ReadOnly<Transform>() }
        };
        _bulletQuery = GetEntityQuery(query);

        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }
    protected override void OnUpdate()
    {
        Entities.With(_bulletQuery).ForEach(
            (Entity entity, Transform transform, ref BulletTag move) =>
            {
                if (move.DontMove)
                    return;
                
                var pos = transform.position;
                pos += move.Speed * Time.DeltaTime * transform.TransformDirection(Vector3.forward);
                transform.position = pos;

                // decrement by time elapsed for one frame
                if (!move.DontDestroy)
                {
                    move.TimeDestroy -= Time.DeltaTime;

                    // if we have timed out, remove the Entity safely
                    if (move.TimeDestroy <= 0)
                    {
                        _entityManager.DestroyEntity(entity);
                        Object.Destroy(transform.gameObject);
                    }
                }
            });
    }

}
