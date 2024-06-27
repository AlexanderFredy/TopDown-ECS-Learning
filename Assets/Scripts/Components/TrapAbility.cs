using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapAbility : CollisionAbility, ICollisionAbility
{
    public Action OnExecute;
    
    public int Damage = 10;
    public float DamageDelay = 1;
    
    private float lastDamageTime = 0;
    private bool _armed;

    new public void Execute()
    {
        foreach (var target in Collisions)
        {
            var targetHealth = target?.gameObject?.GetComponentInParent<CharacterHealth>();

            if (targetHealth != null && _armed && Time.time - lastDamageTime > DamageDelay)
            {
                targetHealth.ApplyDamage(Damage);
                lastDamageTime = Time.time;
                OnExecute?.Invoke();
            }
        }
    }

    public void SetArmed(bool arm) => _armed = arm;
}
