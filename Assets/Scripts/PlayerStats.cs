using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerStats
{  
    public int shotCount;
    public int enemyKill;
    public int health;
    public bool hasBooletReflect;
    
    public string ToExport()
    {
        return JsonUtility.ToJson(this);
    }

    public PlayerStats FromExport(string JSONstring)
    {
        return JsonUtility.FromJson<PlayerStats>(JSONstring);
    }
}
