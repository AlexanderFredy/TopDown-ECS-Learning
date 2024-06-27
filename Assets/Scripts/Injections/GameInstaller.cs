using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField]
    private InventoryViewer _inventoryViewer;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        
        Container.BindInterfacesAndSelfTo<InventoryController>().AsSingle().NonLazy();
        Container.Bind<InventoryViewer>().FromInstance(_inventoryViewer).AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<Level3>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<Level2>().AsSingle().NonLazy();

        InstallExecutionOrder();
    }

    void InstallExecutionOrder()
    {
        Container.BindExecutionOrder<GameManager>(-20);
        Container.BindExecutionOrder<ILevelUp>(-10);       
    }
}



