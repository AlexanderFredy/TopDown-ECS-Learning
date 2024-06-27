using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefBullAbility : CollisionAbility, ICollisionAbility
{
    new public void Execute()
    {
        foreach (var target in Collisions)
        {
            // if (target.CompareTag("Hero"))
            // {
            //     var ba = target.gameObject.GetComponent<ShootAbility>();
            //     if (ba != null)
            //         ba.bulletDestroyAfterCollision = false;
            //
            //     GameManager.S.stat.hasBooletReflect = true;
            // }
        }
    }

}
