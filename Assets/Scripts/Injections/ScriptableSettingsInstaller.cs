using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ScriptableSettingsInstaller", menuName = "Installers/ScriptableSettingsInstaller")]

public class ScriptableSettingsInstaller : ScriptableObjectInstaller<ScriptableSettingsInstaller>
{
    [SerializeField] public GameSettings _gameSettings;
    [SerializeField] public CraftRecipe _craftRecipe;

    public override void InstallBindings()
    {
        Container.BindInstance(_gameSettings);
        Container.BindInstance(_craftRecipe);
    }
}