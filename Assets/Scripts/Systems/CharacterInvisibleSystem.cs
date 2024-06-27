using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class CharacterInvisibleSystem : ComponentSystem
{
    private EntityQuery _invisQuery;

    protected override void OnCreate()
    {
        _invisQuery = GetEntityQuery(ComponentType.ReadOnly<InputData>(), 
            ComponentType.ReadOnly<InvisibleData>(),
            ComponentType.ReadOnly<UserInputData>());
    }
    protected override void OnUpdate()
    {
        Entities.With(_invisQuery).ForEach(
            (Entity entity, UserInputData inputData, ref InputData input) => 
            {
                if (input.Invis > 0f && inputData.InvisibleAction != null && inputData.InvisibleAction is IAbility ability)
                {
                    ability.Execute();
                }
            });
    }

}
