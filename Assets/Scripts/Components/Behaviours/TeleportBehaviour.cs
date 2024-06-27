using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class TeleportBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField]
    private Renderer _meshRenderer;

    [SerializeField]
    private float teleportDuration;

    //private Animator _anim;
    private BehaviourManager _bhmngr;
    //private float _lastAttackTime;
    private Material[] _materials;
    public bool InProgress { get; set; }

    private void Start()
    {
        //_anim = GetComponent<Animator>();
        _bhmngr = GetComponent<BehaviourManager>();

        _materials = _meshRenderer.materials;
    }

    public async void Behave()
    {
        _bhmngr.DontInterrupt = true;

        _bhmngr.Animator.SetTrigger("Teleport");
        await Task.Delay(4000);

        _bhmngr.DontInterrupt = false;
    }

    private void ChangePosition()
    {
        transform.position = Utils.GetPositionInArea();
    }

    public float Evaluate()
    {
        return 0.8f;
    }

}
