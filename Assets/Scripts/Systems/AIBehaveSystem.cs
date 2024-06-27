using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

[UpdateAfter(typeof(AIEvaluateSystem))]
public class AIBehaveSystem : ComponentSystem
{
    private EntityQuery _behaveQuery;
    private EntityManager _entityManager;

    protected override void OnCreate()
    {
        _behaveQuery = GetEntityQuery(ComponentType.ReadOnly<IAgent>());
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }
    protected override void OnUpdate()
    {
        Entities.With(_behaveQuery).ForEach(
            (Entity entity, BehaviourManager manager) =>
            {
                if (manager.IsDead)
                    _entityManager.DestroyEntity(entity);                  
                else if (!manager.DontInterrupt)
                    manager.activeBehaviour?.Behave();
            });
    }
}
