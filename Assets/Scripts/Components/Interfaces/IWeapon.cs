using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public int Damage { get; set; }
    
    public float DamageInterval { get; set; }
    
    public void MakeDamage(CharacterHealth health);
    
    public Vector3 HandlerLocalPos { get; set; }
    
    public Vector3 HandlerLocalRot { get; set; }
    
    public GameObject Owner { get; set; }
}
