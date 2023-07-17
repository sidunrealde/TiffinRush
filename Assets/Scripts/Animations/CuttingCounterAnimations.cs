using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterAnimations : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;

    private const string CUT = "Cut";
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        cuttingCounter.OnCut += CuttingCounterOnCut; 
    }

    private void CuttingCounterOnCut(object sender, System.EventArgs e)
    {
        animator.SetTrigger(CUT);
    }

}
