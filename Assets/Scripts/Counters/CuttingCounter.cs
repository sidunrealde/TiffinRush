using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyCut;
    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }

    public event EventHandler OnCut;

    [SerializeField] private SO_CuttingRecipie[] cuttingRecipieSOArray;

    private int cuttingProgress;

    public override void Interact(PlayerCharacter playerCharacter)
    {
        if (!HasKitchenObject())
        {
            // No kitchen object
            if (playerCharacter.HasKitchenObject())
            {
                if(HasRecipieWithInput(playerCharacter.GetKitchenObject().GetSO_KitchenObject()))
                {
                    // Place kitchen object on counter
                    playerCharacter.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;

                    SO_CuttingRecipie cuttingRecipeSO = GetCuttingRecipieSOWithInput(GetKitchenObject().GetSO_KitchenObject());

                    // Event for progress bar
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    });

                    
                }
                
            }
        }
        else
        {
            // Has Kitchen Object
            if (!playerCharacter.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(playerCharacter);
            }
            else
            {
                // Do plate logic
                if (playerCharacter.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetSO_KitchenObject()))
                    {
                        GetKitchenObject().DestroySelf();
                    }

                }
            }
        }
    }

    public override void DoTask(PlayerCharacter playerCharacter)
    {
        if(HasKitchenObject() && HasRecipieWithInput(GetKitchenObject().GetSO_KitchenObject()))
        {
            // true if kitchen object is place and it can be cut
            cuttingProgress += 1;
            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);
            SO_CuttingRecipie cuttingRecipeSO = GetCuttingRecipieSOWithInput(GetKitchenObject().GetSO_KitchenObject());
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                SO_KitchenObject outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetSO_KitchenObject());
                GetKitchenObject().DestroySelf();

                // Spawn new object
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }

    private bool HasRecipieWithInput(SO_KitchenObject inputKitchenObjectSO)
    {
        SO_CuttingRecipie cuttingRecipeSO = GetCuttingRecipieSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;

    }

    private SO_KitchenObject GetOutputForInput(SO_KitchenObject inputKitchenObjectSO)
    {
        SO_CuttingRecipie cuttingRecipeSO = GetCuttingRecipieSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private SO_CuttingRecipie GetCuttingRecipieSOWithInput(SO_KitchenObject inputKitchenObjectSO)
    {
        foreach (SO_CuttingRecipie cuttingRecipieSO in cuttingRecipieSOArray)
        {
            if (cuttingRecipieSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipieSO;
            }
        }
        return null;
    }
}
