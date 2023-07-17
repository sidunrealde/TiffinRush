using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SO_FryingRecipie : ScriptableObject
{
    public SO_KitchenObject input;
    public SO_KitchenObject display;
    public SO_KitchenObject output;
    public float fryingTime;
}
