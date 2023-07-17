using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{

    [SerializeField] private SO_KitchenObject kitchenObjectSO;


    public override void Interact(PlayerCharacter playerCharacter)
    {
        if (!HasKitchenObject())
        {
            // No kitchen object
            if (playerCharacter.HasKitchenObject())
            {
                // Place kitchen object on counter
                playerCharacter.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else
        {
            // Has Kitchen Object
            if (playerCharacter.HasKitchenObject())
            {
                // player has kitchen object
                // Check if plate
                if (playerCharacter.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetSO_KitchenObject()))
                    {
                        GetKitchenObject().DestroySelf();
                    }

                }
                else
                {
                    // not plate
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // counter has plate
                        if (plateKitchenObject.TryAddIngredient(playerCharacter.GetKitchenObject().GetSO_KitchenObject()))
                        {
                            playerCharacter.GetKitchenObject().DestroySelf() ;
                        }
                    }
                }
                
                // Swap Items
                /*
                KitchenObject playerKitchenObject = playerCharacter.GetKitchenObject();
                playerCharacter.ClearKitchenObject();
                KitchenObject counterKitchenObject = GetKitchenObject();
                ClearKitchenObject();
                counterKitchenObject.SetKitchenObjectParent(playerCharacter);
                playerKitchenObject.SetKitchenObjectParent(this);
                */
            }
            else
            {
                // Pickup Item
                GetKitchenObject().SetKitchenObjectParent(playerCharacter);
            }
        }
    }

}
