using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterAnimations : MonoBehaviour
{

    [SerializeField] private ContainerCounter containerCounter;

    private const string OPEN_CLOSE = "OpenClose";
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        containerCounter.OnPlayerGrabObject += ContainerCounterOnPlayerGrabObject;
    }

    private void ContainerCounterOnPlayerGrabObject(object sender, System.EventArgs e)
    {
        animator.SetTrigger(OPEN_CLOSE);
    }
}
