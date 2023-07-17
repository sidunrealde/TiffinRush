using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance  { get; private set; }

    private void Awake()
    {
        Instance = this; 
    }
    public override void Interact(PlayerCharacter playerCharacter)
    {
        if (playerCharacter.HasKitchenObject())
        {
            if (playerCharacter.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
                playerCharacter.GetKitchenObject().DestroySelf();

            }
        }
    }
}
