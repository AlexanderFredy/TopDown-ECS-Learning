using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class PlayerSettings : ScriptableObject
{
    public int startHealth = 100;
    public float movespeed = 3;
    [Space]
    public float invisDuration = 5f;
    public float invisCooldown = 15f;
}
