using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class AIEvaluateSystem : ComponentSystem
{
    private EntityQuery _evaluateQuery;

    protected override void OnCreate()
    {
        _evaluateQuery = GetEntityQuery(ComponentType.ReadOnly<IAgent>());
    }
    protected override void OnUpdate()
    {
        Entities.With(_evaluateQuery).ForEach(
            (Entity entity, BehaviourManager manager) =>
            {
                if (manager.DontInterrupt)
                    return;
                
                float highScore = float.MinValue;
                manager.activeBehaviour = null;

                foreach (var behaviour in manager.behaviours)
                {
                    if (behaviour is IBehaviour ai)
                    {
                        var curScore = ai.Evaluate();
                        if (curScore > highScore)
                        {
                            highScore = curScore;
                            manager.activeBehaviour = ai;
                        }
                    }
                }
            });
    }
}
