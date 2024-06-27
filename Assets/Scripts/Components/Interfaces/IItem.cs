using System;
using UnityEngine;
using Zenject;

public interface IItem
{
    GameObject Icon2DPref { get; set; }

    InventoryController Inventory { get; }

    public GameObject Owner { get; set; }
}