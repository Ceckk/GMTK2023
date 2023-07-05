using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAPManager : MonoSingleton<IAPManager>, IDetailedStoreListener
{
    private IStoreController _storeController;
    private IExtensionProvider _storeExtensionProvider;

    private bool _skipIAP = false;
    private bool _initializeFailed = false;

    public bool SkipIAP { get => _skipIAP; set => _skipIAP = value; }
    public bool InitializeFailed { get => _initializeFailed; }

    public void Initialize()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //Add products that will be purchasable and indicate its type.
        // builder.AddProduct(goldProductId, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public bool IsInitialized()
    {
        return _skipIAP || (_storeController != null && _storeExtensionProvider != null);
    }

    public void RestorePurchases()
    {
#if UNITY_IOS
        _storeExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions((success, error) =>
        {
            if (success)
            {
                // This does not mean anything was restored,
                // merely that the restoration process succeeded.
                // ProcessPurchase will be called for all the non consumable items it owns
                Debug.Log("restoration completed");
            }
            else
            {
                Debug.Log("restoration failed: " + error);
            }
        });
#else
        Debug.LogWarning("No restore purchases available for this platform, it's probably done automatically during install");
#endif
    }

    public void Purchase(string productId)
    {
        _storeController.InitiatePurchase(productId);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("In-App Purchasing successfully initialized");
        _storeController = controller;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        OnInitializeFailed(error, null);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        var errorMessage = $"Purchasing failed to initialize. Reason: {error}.";

        if (message != null)
        {
            errorMessage += $" More details: {message}";
        }

        Debug.Log(errorMessage);
        _initializeFailed = true;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureDescription: {failureDescription}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        // TODO: receipt validation
        throw new System.NotImplementedException();
    }
}