using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabObject;

    [SerializeField] private SO_KitchenObject kitchenObjectSO;

    public override void Interact(PlayerCharacter playerCharacter)
    {
        if (!playerCharacter.HasKitchenObject())
        {
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, playerCharacter);
            
            OnPlayerGrabObject?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            // Do Nothing
        }

    }


}
