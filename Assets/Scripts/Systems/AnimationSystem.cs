using Unity.Entities;
using UnityEngine;

public class AnimationSystem : ComponentSystem
{
    private EntityQuery _animQuery;

    protected override void OnCreate()
    {
        _animQuery = GetEntityQuery(ComponentType.ReadOnly<AnimationData>(),
            ComponentType.ReadOnly<InputData>(),ComponentType.ReadOnly<Animator>());
    }
    protected override void OnUpdate()
    {
        Entities.With(_animQuery).ForEach(
            (Entity entity, Animator anim, ref InputData inputData) => 
            {
                if (inputData.Move.x == 0 && inputData.Move.y == 0)
                    anim.SetBool("Move", false);
                else
                    anim.SetBool("Move", true);
                
                if (inputData.Melee >= 0.5f)
                    anim.SetTrigger("Attack");
                
                anim.SetFloat("RunMultiplier",1f +2*inputData.Boost);
            });
    }

}
