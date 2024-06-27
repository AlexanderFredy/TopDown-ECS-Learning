using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot: MonoBehaviour
{  
    public IItem currentItem;
    public GameObject Icon2D;

    private Outline _outliner;

    public bool IsSelected { 
        set { _isSelected = value;
            if (_outliner != null)
                _outliner.enabled = value;
        }
        get { return _isSelected; }   
    }

    private bool _isSelected;

    private void Awake()
    {
        _outliner = GetComponent<Outline>();
        IsSelected = false;
    }

    public void OnSlotPointerDown()
    {
        if (currentItem != null) { 
            IsSelected =  !IsSelected;
        }
    }
}