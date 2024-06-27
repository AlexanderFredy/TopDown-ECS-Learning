using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using Zenject;

public class MyIAPManager : MonoBehaviour, IDetailedStoreListener
{
    [SerializeField] private BaseItem _redPref;
    [SerializeField] private BaseItem _bluePref;
    [SerializeField] private BaseItem _greenPref;

    private Dictionary<string, BaseItem> _purchIdPrefabs;

    [Inject]
    private GameManager _gameManager;
    [Inject]
    private DiContainer _diContainer;

    private IStoreController controller;
    private IExtensionProvider extensions;

    private void Awake()
    {
        _purchIdPrefabs = new Dictionary<string, BaseItem> {
            {"RedPerk", _redPref},
             {"BluePerk", _bluePref},
              {"GreenPerk", _greenPref}
        };
    }

    private void Start()
    {            
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct("RedPerk", ProductType.Consumable);
        builder.AddProduct("BluePerk", ProductType.Consumable);
        builder.AddProduct("GreenPerk", ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    /// <summary>
    /// Called when Unity IAP is ready to make purchases.
    /// </summary>
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        this.controller = controller;
        this.extensions = extensions;

        print("Success initialization Store Listener");
    }

    /// <summary>
    /// Called when Unity IAP encounters an unrecoverable initialization error.
    ///
    /// Note that this will not be called if Internet is unavailable; Unity IAP
    /// will attempt initialization until it becomes available.
    /// </summary>
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        print("Initialize failed" + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        print("Initialize failed" + error + message);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        print("Purchase " + product + " failed" + failureDescription);
    }

    /// <summary>
    /// Called when a purchase fails.
    /// </summary>
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        print("Purchase " + product + " failed" + failureReason);
    }

    /// <summary>
    /// Called when a purchase completes.
    ///
    /// May be called at any time after OnInitialized().
    /// </summary>
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        var product = purchaseEvent.purchasedProduct;
        print("Purchase Complete " + product.definition.id);
        GetPerk(_purchIdPrefabs[product.definition.id]);

        return PurchaseProcessingResult.Complete;
    }

    public void BuyPerk(string productID)
    {
        controller.InitiatePurchase(productID);
    }

    public void GetPerk(BaseItem prefab)
    {
        if (_gameManager.CurrentPlayer != null)
        {
            var perk = _diContainer.InstantiatePrefab(prefab);
            perk.GetComponent<BaseItem>().AddToInventory();
            print(perk + " was added");
        }
        else
            print("There is not player"); 
    }

    public void GetRandomPerk()
    {
        var perk = _purchIdPrefabs.ElementAt(Random.Range(0, _purchIdPrefabs.Count));
        GetPerk(perk.Value);
    }
}
