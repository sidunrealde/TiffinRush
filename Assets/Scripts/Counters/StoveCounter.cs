using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burnt,
    }

    [SerializeField] private SO_FryingRecipie[] fryingRecipiesSOArray;
    [SerializeField] private SO_BurnRecipie[] burnRecipiesSOArray;

    private State state;
    private float fryingTimer;
    private float burnTimer;
    private SO_FryingRecipie fryingRecipeSO;
    private SO_BurnRecipie burnRecipieSO;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTime
                    });

                    if (fryingTimer > fryingRecipeSO.fryingTime)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        state = State.Fried;
                        burnTimer = 0f;
                        burnRecipieSO = GetBurnRecipieSOWithInput(GetKitchenObject().GetSO_KitchenObject());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state=state
                        });
                    }
                    break;
                case State.Fried:
                    burnTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = burnTimer / burnRecipieSO.burnTimerMax
                    });

                    if (burnTimer > burnRecipieSO.burnTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burnRecipieSO.output, this);
                        state = State.Burnt;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                    break;
                case State.Burnt:
                    break;

            }
        }


    }
    
    public override void Interact(PlayerCharacter playerCharacter)
    {
        if (!HasKitchenObject())
        {
            // No kitchen object
            if (playerCharacter.HasKitchenObject())
            {
                if (HasRecipieWithInput(playerCharacter.GetKitchenObject().GetSO_KitchenObject()))
                {
                    fryingRecipeSO = GetfryingRecipieSOWithInput(playerCharacter.GetKitchenObject().GetSO_KitchenObject());
                    // Place kitchen object on counter
                    SO_KitchenObject displayKitchenObjectSO = GetDisplayForInput(playerCharacter.GetKitchenObject().GetSO_KitchenObject());
                    KitchenObject.SpawnKitchenObject(displayKitchenObjectSO,this);
                    playerCharacter.GetKitchenObject().DestroySelf();

                    state = State.Frying;
                    fryingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTime
                    });
                }

            }
        }
        else
        {
            // Has Kitchen Object
            if (!playerCharacter.HasKitchenObject())
            {
                if (state != State.Frying)
                {
                    GetKitchenObject().SetKitchenObjectParent(playerCharacter);
                    state = State.Idle;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = 0f
                    });
                }
            }
            else
            {
                // do plate logic
                if (playerCharacter.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetSO_KitchenObject()))
                    {
                        GetKitchenObject().DestroySelf();

                        state = State.Idle;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }

                }
            }
        }
    }

    private bool HasRecipieWithInput(SO_KitchenObject inputKitchenObjectSO)
    {
        SO_FryingRecipie fryingRecipeSO = GetfryingRecipieSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;

    }

    private SO_KitchenObject GetOutputForInput(SO_KitchenObject inputKitchenObjectSO)
    {
        SO_FryingRecipie fryingRecipeSO = GetfryingRecipieSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private SO_KitchenObject GetDisplayForInput(SO_KitchenObject inputKitchenObjectSO)
    {
        SO_FryingRecipie fryingRecipeSO = GetfryingRecipieSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.display;
        }
        else
        {
            return null;
        }
    }

    private SO_FryingRecipie GetfryingRecipieSOWithInput(SO_KitchenObject inputKitchenObjectSO)
    {
        foreach (SO_FryingRecipie fryingRecipieSO in fryingRecipiesSOArray)
        {
            if (fryingRecipieSO.input == inputKitchenObjectSO)
            {
                return fryingRecipieSO;
            }
        }
        return null;
    }

    private SO_BurnRecipie GetBurnRecipieSOWithInput(SO_KitchenObject inputKitchenObjectSO)
    {
        foreach (SO_BurnRecipie burnRecipieSO in burnRecipiesSOArray)
        {
            if (burnRecipieSO.input == inputKitchenObjectSO)
            {
                return burnRecipieSO;
            }
        }
        return null;
    }

    public bool IsFried()
    {
        return state == State.Fried;
    }
}
