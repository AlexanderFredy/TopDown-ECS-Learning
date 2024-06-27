using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using UnityEngine;

public class DogMoveSystem : ComponentSystem
{
    private EntityQuery _query;

    protected override void OnCreate()
    {
        _query = GetEntityQuery(ComponentType.ReadOnly<DogMoveComponent>());

    }

    protected override void OnUpdate()
    {
        Entities.With(_query).ForEach((Entity entity, Transform transform, DogMoveComponent dogMove) =>
        {
            Vector3 p = transform.position;
            p.y += dogMove.moveSpeed / 1000;
            transform.position = p;
        });

    }
}
