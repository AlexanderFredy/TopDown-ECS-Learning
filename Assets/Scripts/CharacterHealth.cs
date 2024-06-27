using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    public event Action OnHealthChange;
    public event Action OnKill;
    public event Action OnGetHit;
    
    public int Health { 
        get => _health;
        private set {
            if (value <= 0)
            {
                OnKill?.Invoke();
                return;
            }

            if (value < _health) OnGetHit?.Invoke();
            
            _health = Mathf.Max(0,value);
            OnHealthChange?.Invoke();
        }
    }
    
    [SerializeField]
    private int _health = 100;

    public void ApplyDamage(int dmg)
    {
        Health -= dmg;
    }
    
    public void AddHealth(int hp)
    {
        Health += hp;
    }
    
    public void SetHealth(int hp)
    {
        Health = hp;
    }
}
