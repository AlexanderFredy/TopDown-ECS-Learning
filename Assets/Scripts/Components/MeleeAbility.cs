using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class MeleeAbility : MonoBehaviour, IAbility
{
    public WeaponAbility curWeapon;
    public float shootDelay;

    [SerializeField] private AK.Wwise.Event strikeEvent;

    private float lastAttackTime = 0;

    public void Execute()
    {
        if (Time.time - lastAttackTime < shootDelay) return;
            
        lastAttackTime = Time.time;
        strikeEvent.Post(gameObject);
    }
    
}
