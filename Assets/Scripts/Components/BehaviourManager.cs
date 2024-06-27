using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System;
using UnityEngine.UI;
using UniRx;
using Zenject;

public class BehaviourManager : MonoBehaviour, IConvertGameObjectToEntity
{
    public EnemyType type;
    public CharacterHealth health;
    public Text hpLablePref;
    public List<MonoBehaviour> behaviours;
    public bool DontInterrupt;
    public IBehaviour activeBehaviour;
    public bool IhaveWeapon;
    public GameObject deathEffect;
    public Animator Animator;

    [HideInInspector]
    public Transform partOfBodyForWeaponPrefab;

    public bool IsDead { private set; get; }

    [Inject]
    private GameManager _gameManager;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponent<IAgent>(entity);      
    }

    private Text hpLable;

    private void Start()
    {
        partOfBodyForWeaponPrefab = transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/" +
            "mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandIndex1");
        
        var Lable = Instantiate(hpLablePref, Vector3.zero, Quaternion.identity);
        Lable.transform.SetParent(GameObject.Find("DynamicIndicators").transform);
        Lable.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(0f, 140f);
        Lable.text = health.Health.ToString();
        hpLable = Lable;
        health.OnHealthChange += () => hpLable.text = health.Health.ToString();
            
        health.OnKill += KillCharacter;

        var cam = Camera.main;
        var hpTrans = hpLable.GetComponent<RectTransform>();
        Observable.EveryUpdate().Subscribe(_ => 
        { 
            hpTrans.position = cam.WorldToScreenPoint(transform.position) + new Vector3(0f, 140f); 
        }).AddTo(this);
    }

    private void KillCharacter()
    {
        if (!IsDead)
        {
            IsDead = true;
            Utils.SetGameLayerRecursive(gameObject,10);//for all childs
            Destroy(this.gameObject, 4f);
            Destroy(hpLable);
            ShowDeathEffect();
            
            if (_gameManager.CurrentPlayer != null && _gameManager.CurrentPlayer.TryGetComponent(out CharacterData characterData))
            {
                characterData.AddScore(10);
            }
        }
    }

    private void ShowDeathEffect()
    {
        switch (type)
        {
            case EnemyType.Mag:
                if (deathEffect != null)
                {
                    Instantiate(deathEffect, transform.position, Quaternion.identity);
                    Animator.SetTrigger("Death");
                }
                break;
            case EnemyType.Knight:
                //отпускаем оружие, чтобы оно оставалось в сцене
                var weapon = gameObject.GetComponentInChildren<WeaponAbility>();
                if (weapon != null)
                {
                    weapon.transform.parent = null;
                    weapon.ClearCollisionLayerAndOwner();
                    if (weapon.TryGetComponent(out Rigidbody rb))
                    {
                        rb.isKinematic = false;
                        rb.useGravity = true;
                    }
                }

                var rdc = gameObject.GetComponentInParent<RagdollControl>();
                if (rdc != null)
                {
                    rdc.RagdollEnable();                 
                }
                break;
        }
    }

    private void OnDestroy()
    {
        health.OnKill -= KillCharacter;
    }
}

public struct IAgent: IComponentData 
{
}

public enum EnemyType
{
    Knight,
    Mag
}
