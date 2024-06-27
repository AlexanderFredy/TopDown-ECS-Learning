using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreController : MonoBehaviour
{
    public void OnPurchesedCompleted(string productID)
    {
        print("Purchesed " + productID);
    }
}
