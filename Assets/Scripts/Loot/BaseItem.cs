using UnityEngine;
using Zenject;

public abstract class BaseItem : CollisionAbility, IItem
{
    [field: SerializeField]
    public GameObject Icon2DPref { get; set; }

    [SerializeField]
    private AK.Wwise.Event pickupEvent;

    public InventoryController Inventory { get; private set; }

    public GameObject Owner { get; set; } = null;

    private GameManager _gameManager;

    [Inject]
    public void Construct(GameManager gameManager, InventoryController inventoryController)
    {
        _gameManager = gameManager;
        Inventory = inventoryController;
    }

    public override void Execute()
    {
        foreach (var target in Collisions)
        {
            if (target.gameObject.TryGetComponent<CharacterData>(out var targetCharacterData))
            {
                pickupEvent.Post(gameObject);
                AddToInventory();              
            }
        }
    }

    public void AddToInventory()
    {
        DestroyAfterCollision = false;
        gameObject.SetActive(false);
        gameObject.transform.position = Inventory.Store3DItems.position;
        gameObject.transform.parent = Inventory.Store3DItems;

        Inventory.AddToInventory(this);

        if (_gameManager.CurrentPlayer != null)
            Owner = _gameManager.CurrentPlayer;
    }
}