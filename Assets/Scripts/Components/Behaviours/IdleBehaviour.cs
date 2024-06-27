using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField]
    private float idleDelay;

    private float duration = 0f;

    public bool InProgress { get ; set; }

    public void Behave()
    {
        duration += 0.01f;
    }

    public float Evaluate()
    {
        if (duration > idleDelay)//пора двигаться
        {
            duration = 0f;
            return 0.5f;
        }
        else
            return 0.9f;
    }

}
