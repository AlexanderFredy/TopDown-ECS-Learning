using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class CharacterMoveSystem : ComponentSystem
{
    private EntityQuery _moveQuery;

    protected override void OnCreate()
    {
        _moveQuery = GetEntityQuery(ComponentType.ReadOnly<InputData>(),
            ComponentType.ReadOnly<UserInputData>(),
            ComponentType.ReadOnly<MoveData>(), 
            ComponentType.ReadOnly<Transform>());
    }
    protected override void OnUpdate()
    {
        Entities.With(_moveQuery).ForEach(
            (Entity entity, Transform transform, ref InputData inputData, ref MoveData move) => 
            {
                float boostMultiplier = 1f + 2*inputData.Boost;

                var pos = transform.position;
                pos += new Vector3(inputData.Move.x, 0, inputData.Move.y) * move.Speed * boostMultiplier * Time.DeltaTime;
                transform.position = pos;

                Vector3 moveDirection = new Vector3(inputData.Move.x, 0, inputData.Move.y).normalized;

                if (moveDirection != Vector3.zero)
                {
                    Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, move.RotationSpeed* Time.DeltaTime);
                }
            });
    }

}
