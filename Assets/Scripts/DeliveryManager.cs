using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private SO_RecipeList recipeListSO;
    [SerializeField] private float spawnRecipeTimerMax;

    private List<SO_Recipes> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private int waitingRecipesMax = 4;
    private int successfulDeliveries;

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<SO_Recipes>();
    }



    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer < 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            if (waitingRecipeSOList.Count < waitingRecipesMax)
            {
                SO_Recipes waitingRecipeSO = recipeListSO.recipeListSO[UnityEngine.Random.Range(0, recipeListSO.recipeListSO.Count)];
                waitingRecipeSOList.Add(waitingRecipeSO);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
            
        }
    }
    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            SO_Recipes waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                // has same number of ingredients
                bool plateContentMatchesRecipe = true;
                foreach(SO_KitchenObject recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    bool ingredientFound = false;
                    foreach(SO_KitchenObject plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        // cycling through ingredients
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            ingredientFound= true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        // ingredient not on plate
                        plateContentMatchesRecipe= false;
                    }
                }
                if (plateContentMatchesRecipe)
                {
                    // player delivered correct recipe
                    successfulDeliveries++;

                    waitingRecipeSOList.RemoveAt(i);

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);

                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
               
            }
            // Did not deliver correct recipe
            OnRecipeFailed?.Invoke(this, EventArgs.Empty);
        }
    }

    public List<SO_Recipes> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    public int GetSuccessfulDeliveriesAmount()
    {
        return successfulDeliveries;
    }
}
