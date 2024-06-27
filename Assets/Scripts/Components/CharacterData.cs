using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterData : MonoBehaviour
{
    [ShowInInspector]
    private int currentLevel = 1;
    [ShowInInspector]
    private int scoreToNextLevel = 20;
    [ShowInInspector]
    private int score = 0;

    public List<IConsumeEffect> currentConsumeEffects = new();

    [Inject]
    private List<ILevelUp> _levels;

    public void AddScore(int amount)
    {
        this.score += amount;
        if (score >= scoreToNextLevel)
            LevelUp();
    }

    private void LevelUp()
    {
        currentLevel++;
        scoreToNextLevel *= 2;
        foreach (var item in _levels)
        {
            if (item.LevelNumber == currentLevel)
                item.LevelUp();
        }
    }
}
