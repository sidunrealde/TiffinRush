using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class UIStoveBurnWarning : MonoBehaviour
{
    [SerializeField] StoveCounter stoveCounter;

    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounterOnProgressChanged;
        Hide();
    }

    private void StoveCounterOnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burnShowProgressAmount = .25f;
        bool show = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;
        if (show)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
