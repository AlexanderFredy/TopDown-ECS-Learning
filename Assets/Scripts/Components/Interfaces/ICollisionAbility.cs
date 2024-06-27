using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollisionAbility: IAbility
{
    public List<Collider> Collisions { get; set; }

    public bool DestroyAfterCollision { get; set; }
    
}
