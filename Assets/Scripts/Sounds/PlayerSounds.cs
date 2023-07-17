using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private float footstepTimerMax = 0.4f;

    private PlayerCharacter playerCharacter;
    private float footstepTimer;
    

    private void Awake()
    {
        playerCharacter = GetComponent<PlayerCharacter>();
    }
    private void Update()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer < 0)
        {
            footstepTimer = footstepTimerMax;

            if (playerCharacter.IsWalking())
            {
                float volume = 3f;
                SoundManager.Instance.playFootstepsSound(playerCharacter.transform.position, volume);
            }
           
        }
    }
}
