using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerCharacter player;
    private Animator animator;
    private const string IS_WALKING = "IsWalking";
    private void Awake()
    {
        animator = GetComponent<Animator>();

       
    }

    private void Update()
    {
        animator.SetBool(IS_WALKING, player.IsWalking());
        if (player.HasKitchenObject())
        {
            animator.SetLayerWeight(1, 1f);
        }
        else
        {
            animator.SetLayerWeight(1, 0f);
        }
        
    }
}
