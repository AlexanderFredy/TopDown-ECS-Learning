using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    public GameObject playerPrefab;
    public GameObject healthPrefab;

    [Space]
    public GameObject enemyPrefabKnight;
    public GameObject enemyPrefabWarlock;
    public Transform enemySpawner;
}

