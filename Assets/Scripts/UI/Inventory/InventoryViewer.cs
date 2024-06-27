using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using Zenject;

public class InventoryViewer : MonoBehaviour
{
    [SerializeField]
    private InventorySlot slotPref;

    private InventoryController _controller;

    private UIControls _uiControl;
    private InventorySlot[] _slots;

    [Inject]
    public void Construct(InventoryController invController)
    {
        _slots = transform.Find("Grid").GetComponentsInChildren<InventorySlot>();      

        _uiControl = new();
        _uiControl.Enable();

        _uiControl.UIActionMap.InventoryCloseOpen.performed += OnInventoryCloseOpen;

        _controller = invController;
        _controller.OnInventoryCleared += ClearSlots;
        _controller.OnItemAdded += AddToSlot;
        _controller.OnItemRemoved += RemoveFromSlot;
    }

    public void ConsumeButton() 
    {
        _controller.Consume(_slots);
    }

    public void CraftButton()
    {
        _controller.Craft(_slots);
    }

    private void AddToSlot(int i, IItem item)
    {
        if (_slots[i].Icon2D != null)
            Destroy(_slots[i].Icon2D);

        _slots[i].currentItem = item;
        _slots[i].Icon2D = Instantiate(item.Icon2DPref, _slots[i].transform);          
    }

    private void RemoveFromSlot(int i, IItem item)
    {
        if (_slots[i].Icon2D != null)
            Destroy(_slots[i].Icon2D);

        _slots[i].currentItem = null;
        _slots[i].IsSelected = false;
    }

    private void ClearSlots()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].Icon2D != null)
                Destroy(_slots[i].Icon2D);

            _slots[i].currentItem = null;
            _slots[i].IsSelected = false;
        }     
    }

    private void OnDestroy()
    {
        _uiControl.UIActionMap.InventoryCloseOpen.performed -= OnInventoryCloseOpen;
        _controller.OnInventoryCleared -= ClearSlots;
        _controller.OnItemAdded -= AddToSlot;
        _controller.OnItemRemoved -= RemoveFromSlot;
    }

    private void OnInventoryCloseOpen(InputAction.CallbackContext context)
    {
        if (gameObject.activeSelf)
            CloseIventory();
        else
            OpenIventory();
    }

    public void OpenIventory()
    {
        gameObject.SetActive(true);
    }

    public void CloseIventory()
    {
        gameObject.SetActive(false);

        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].IsSelected = false;
        }
    }
}
