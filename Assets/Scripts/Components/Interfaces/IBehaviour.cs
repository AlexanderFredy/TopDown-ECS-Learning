using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBehaviour
{
    float Evaluate();
    void Behave();

    bool InProgress { get; set; }
}
