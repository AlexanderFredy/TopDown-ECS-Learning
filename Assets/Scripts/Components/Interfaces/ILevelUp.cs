using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface ILevelUp: IInitializable
{
    public int LevelNumber { get; set;}
    void LevelUp();
}
