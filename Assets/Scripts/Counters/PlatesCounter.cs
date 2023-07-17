using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private float spawnPlateTimerMax;
    [SerializeField] private SO_KitchenObject plateKitchenObjectSO;

    private float spawnPlateTimer;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;
    

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer  > spawnPlateTimerMax )
        {
            spawnPlateTimer = 0;

            if (platesSpawnedAmount < platesSpawnedAmountMax )
            {
                platesSpawnedAmount++;

                OnPlateSpawned?.Invoke( this, EventArgs.Empty );
            }
        }
    }

    public override void Interact(PlayerCharacter playerCharacter)
    {
        if (!playerCharacter.HasKitchenObject())
        {
            if (platesSpawnedAmount > 0)
            {
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, playerCharacter);
                platesSpawnedAmount--;
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
            
        }
        
    }
}
