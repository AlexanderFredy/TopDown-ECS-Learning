using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Zenject;

public class ShootAbility : MonoBehaviour, IAbility
{
    public WeaponAbility bulletPref;
    public float shootDelay;

    public bool bulletDontDestroyAfterCollision;
    public bool bulletDontMoveAfterCollision;
    
    private float lastShootTime = 0;
    private WeaponAbility curBullet;

    public void Execute()
    {
        if (Time.time - lastShootTime < shootDelay) return;
        lastShootTime = Time.time;

        if (bulletPref != null)
        {
            var t = transform;
            Vector3 starPosition = t.position;
            starPosition.y += 1f;

            curBullet = Instantiate(bulletPref, starPosition, t.rotation);
            curBullet.SetCollisionLayerAndOwner(this.gameObject);
            curBullet.DestroyAfterCollision = !bulletDontDestroyAfterCollision;
            curBullet.DontMoveAfterCollision = bulletDontMoveAfterCollision;
            curBullet.IsBullet = true;
            
            //GameManager.S.stat.shotCount++;
        }
        else
            Debug.Log("No bullet prefab");
    }
    
}
