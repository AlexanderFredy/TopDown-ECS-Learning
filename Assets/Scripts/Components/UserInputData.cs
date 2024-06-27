using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using UniRx;

public class UserInputData : MonoBehaviour, IConvertGameObjectToEntity
{
    public float speed;
    public float rotationSpeed;

    public MonoBehaviour MeleeAction;
    public MonoBehaviour ShootAction;
    public MonoBehaviour InvisibleAction;

    [SerializeField]
    private AK.Wwise.Event stepEvent;
    [SerializeField]
    private AK.Wwise.RTPC speedParameter;
    private bool _stepSoundsIsPlaying;

    private Animator _animator;

    public Entity MyEntity { get; private set; }

    private void Start()
    {
        Observable.EveryUpdate().Subscribe(_ =>
        {
            if (MyEntity != null)
            {              
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                var input = entityManager.GetComponentData<InputData>(MyEntity);
                if (input.Move.Equals(float2.zero))
                {         
                    stepEvent.Stop(gameObject);

                    _stepSoundsIsPlaying = false;
                }
                else if (!_stepSoundsIsPlaying)
                {                   
                    stepEvent.Post(gameObject);

                    _stepSoundsIsPlaying = true;
                }

                speedParameter.SetGlobalValue(input.Boost * 100);
            }
        });
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new InputData { IsDead = false });
        dstManager.AddComponentData(entity, new MoveData { Speed = speed, RotationSpeed = rotationSpeed });

        if (MeleeAction != null && MeleeAction is IAbility)
        {
            dstManager.AddComponentData(entity, new MeleeData());
        }
        
        if (ShootAction != null && ShootAction is IAbility)
        {
            dstManager.AddComponentData(entity, new ShootData());
        }

        if (InvisibleAction != null && InvisibleAction is IAbility)
        {
            dstManager.AddComponentData(entity, new InvisibleData());
        }

        if (GetComponent<Animator>() != null)
        {
            dstManager.AddComponentData(entity, new AnimationData());
        }

        MyEntity = entity;
    }
}

public struct InputData: IComponentData
{
    public float2 Move;
    public float Melee;
    public float Shoot;
    public float Boost;
    public float Invis;
    public bool IsDead;
}

public struct MoveData : IComponentData
{
    public float Speed;
    public float RotationSpeed;
}

public struct MeleeData : IComponentData
{

}

public struct ShootData : IComponentData
{

}

public struct InvisibleData : IComponentData
{

}

public struct AnimationData : IComponentData
{

}
