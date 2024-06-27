using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAbility : CollisionAbility, ICollisionAbility
{
    public int HealthPoint = 10;

    new public void Execute()
    {
        foreach (var target in Collisions)
        {
            var targetHealth = target?.gameObject?.GetComponent<CharacterHealth>();

            if (targetHealth != null)
            {
                targetHealth.AddHealth(HealthPoint);
                DestroyAfterCollision = true;
            }
        }
    }

}
