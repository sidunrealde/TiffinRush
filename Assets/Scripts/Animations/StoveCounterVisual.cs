using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private GameObject burnerGameObject;
    [SerializeField] private GameObject particleGameObject;
    [SerializeField] private StoveCounter stoveCounter;

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounterOnStateChanged;
    }

    private void StoveCounterOnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool showVisual = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;

        burnerGameObject.SetActive(showVisual);
        particleGameObject.SetActive(showVisual);
    }
}
