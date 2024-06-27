using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Entities;
using Unity.Mathematics;
using Zenject;
using System;

[UpdateAfter(typeof(AnimationSystem))]
public class UserInputSystem : ComponentSystem
{
    private EntityQuery _inputQuery;
    private EntityManager _entityManager;

    private InputAction _moveAction;
    private InputAction _meleeAction;
    private InputAction _shootAction;
    private InputAction _boostAction;
    private InputAction _invisAction;
    private InputAction _quitAction;

    private float2 _moveInput;
    private float _meleeInput;
    private float _shootInput;
    private float _boostInput;
    private float _invisInput;
    private float _quitInput;

    protected override void OnCreate()
    {
        _inputQuery = GetEntityQuery(ComponentType.ReadOnly<InputData>());
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    protected override void OnStartRunning()
    {
        _moveAction = new InputAction("move", binding: "Gamepad/rightStick");
        _moveAction.AddCompositeBinding("Dpad")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        _moveAction.performed += context => { _moveInput = context.ReadValue<Vector2>(); };
        _moveAction.started += context => { _moveInput = context.ReadValue<Vector2>(); };
        _moveAction.canceled += context => { _moveInput = context.ReadValue<Vector2>(); };
        _moveAction.Enable();
        
        _meleeAction = new InputAction("melee", binding: "<Keyboard>/m");

        _meleeAction.performed += context => { _meleeInput = context.ReadValue<float>(); };
        _meleeAction.started += context => { _meleeInput = context.ReadValue<float>(); };
        _meleeAction.canceled += context => { _meleeInput = context.ReadValue<float>(); };
        _meleeAction.Enable();

        _shootAction = new InputAction("shoot", binding: "<Keyboard>/Space");

        _shootAction.performed += context => { _shootInput = context.ReadValue<float>(); };
        _shootAction.started += context => { _shootInput = context.ReadValue<float>(); };
        _shootAction.canceled += context => { _shootInput = context.ReadValue<float>(); };
        _shootAction.Enable();

        _boostAction = new InputAction("boost", binding: "<Keyboard>/rightShift");

        _boostAction.performed += context => { _boostInput = context.ReadValue<float>(); };
        _boostAction.started += context => { _boostInput = context.ReadValue<float>(); };
        _boostAction.canceled += context => { _boostInput = context.ReadValue<float>(); };
        _boostAction.Enable();

        _invisAction = new InputAction("invis", binding: "<Keyboard>/i");

        _invisAction.performed += context => { _invisInput = context.ReadValue<float>(); };
        _invisAction.started += context => { _invisInput = context.ReadValue<float>(); };
        _invisAction.canceled += context => { _invisInput = context.ReadValue<float>(); };
        _invisAction.Enable();

        _quitAction = new InputAction("quit", binding: "<Keyboard>/escape");

        _quitAction.performed += context => { _quitInput = context.ReadValue<float>(); };
        _quitAction.started += context => { _quitInput = context.ReadValue<float>(); };
        _quitAction.canceled += context => { _quitInput = context.ReadValue<float>(); };
        _quitAction.Enable();
    }

    protected override void OnStopRunning()
    {
        _moveAction.Disable();
        _meleeAction.Disable();
        _shootAction.Disable();
        _boostAction.Disable();
        _quitAction.Disable();
        _invisAction.Disable();
    }

    protected override void OnUpdate()
    {
        Entities.With(_inputQuery).ForEach(
            (Entity entity, ref InputData inputData) =>
            { 
                if (inputData.IsDead)
                {
                    _entityManager.DestroyEntity(entity);
                }
                else
                {
                    inputData.Move = _moveInput;
                    inputData.Melee = _meleeInput;
                    inputData.Shoot = _shootInput;
                    inputData.Boost = _boostInput;
                    inputData.Invis = _invisInput;
                }
            });

        if (_quitInput > 0)
        {
            Application.Quit();
        }
    }
}
