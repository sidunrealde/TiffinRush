using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlateIconSingle : MonoBehaviour
{
    [SerializeField] private Image image;
       public void SetKitchenObjectSO(SO_KitchenObject kitchenObjectSO)
    {
        image.sprite = kitchenObjectSO.sprite;
    }
}
