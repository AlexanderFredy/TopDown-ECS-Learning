using UnityEngine;
using Zenject;

public class Level3 : ILevelUp
{
    [Inject]
    private GameManager _gameManager;

    public int LevelNumber { get; set; }

    public void LevelUp()
    {
        var curPlayer = _gameManager.CurrentPlayer;
        if (curPlayer != null && curPlayer.TryGetComponent(out CharacterHealth health))
            health.SetHealth(110);
    }

    public void Initialize()
    {
        LevelNumber = 3;
        
    }
}

public class Level2 : ILevelUp
{
    [Inject]
    private GameManager _gameManager;

    public int LevelNumber { get; set;}

    public void LevelUp()
    {
        var curPlayer = _gameManager.CurrentPlayer;
        if (curPlayer != null && curPlayer.TryGetComponent(out UserInputData userInputData))
            userInputData.speed += 2;
    }

    public void Initialize()
    {
        LevelNumber = 2;
        
    }
}