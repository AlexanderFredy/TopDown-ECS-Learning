using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class CharacterMeleeSystem : ComponentSystem
{
    private EntityQuery _shootQuery;

    protected override void OnCreate()
    {
        _shootQuery = GetEntityQuery(ComponentType.ReadOnly<InputData>(), 
            ComponentType.ReadOnly<MeleeData>(),
            ComponentType.ReadOnly<UserInputData>());
    }
    protected override void OnUpdate()
    {
        Entities.With(_shootQuery).ForEach(
            (Entity entity, UserInputData inputData, ref InputData input) => 
            {
                if (input.Melee > 0f && inputData.MeleeAction != null && inputData.MeleeAction is IAbility ability)
                {
                    ability.Execute();
                }
            });
    }

}
