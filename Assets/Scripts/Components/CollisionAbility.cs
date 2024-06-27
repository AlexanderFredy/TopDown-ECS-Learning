using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class CollisionAbility : MonoBehaviour, IConvertGameObjectToEntity, ICollisionAbility
{       
    public Collider Collider;
    public List<Collider> Collisions { get; set; }
    
    public bool DestroyAfterCollision { get; set; }
    public bool DontMoveAfterCollision { get; set; }
    
    public Entity MyEntity { get; private set; }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        float3 position = gameObject.transform.position;
        switch (Collider)
        {
            case SphereCollider sphere:
                sphere.ToWorldSpaceSphere(out var sphereCenter, out var sphereRadius);
                dstManager.AddComponentData(entity, new ActorColliderData
                {
                    ColliderType = ColliderType.Sphere,
                    SphereCenter = sphereCenter - position,
                    SphereRadius = sphereRadius,
                    initialTakeOff = true,
                    layerMask = Utils.GetLayerMask(Utils.CollisionMatrix,this.gameObject.layer)
                });
                break;
            case CapsuleCollider capsule:
                capsule.ToWorldSpaceCapsule(out var capsuleStart, out var capsuleEnd, out var capsuleRadius);
                dstManager.AddComponentData(entity, new ActorColliderData
                {
                    ColliderType = ColliderType.Capsule,
                    CapsuleStart = capsuleStart - position,
                    CapsuleEnd = capsuleEnd - position,
                    CapsuleRadius = capsuleRadius,
                    initialTakeOff = true,
                    layerMask = Utils.GetLayerMask(Utils.CollisionMatrix,this.gameObject.layer)
                });
                break;
            case BoxCollider box:
                box.ToWorldSpaceBox(out var boxCenter, out var boxHalfExtens, out var boxOrientation);
                dstManager.AddComponentData(entity, new ActorColliderData
                {
                    ColliderType = ColliderType.Box,
                    BoxCenter = boxCenter - position,
                    BoxHalfExtens = boxHalfExtens,
                    BoxOrientation = boxOrientation,
                    initialTakeOff = true,
                    layerMask = Utils.GetLayerMask(Utils.CollisionMatrix,this.gameObject.layer)
                });
                break;
        }

        //Collider.enabled = false;
        MyEntity = entity;
        AddDataToComponentsAfterInstansing();
    }

    protected virtual void AddDataToComponentsAfterInstansing()
    {
        //print(0);
    }

    public virtual void Execute()
    {
        Debug.Log("HIT");
    }
}

public struct ActorColliderData: IComponentData
{
    public ColliderType ColliderType;
    public float3 SphereCenter;
    public float SphereRadius;
    public float3 CapsuleStart;
    public float3 CapsuleEnd;
    public float CapsuleRadius;
    public float3 BoxCenter;
    public float3 BoxHalfExtens;
    public quaternion BoxOrientation;
    public bool initialTakeOff;
    public int layerMask;
}

public enum ColliderType
{
    Sphere = 0,
    Capsule = 1,
    Box = 2
}