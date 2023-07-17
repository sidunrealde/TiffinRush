using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private SO_AudioClips audioClipsSO;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManagerOnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManagerOnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounterOnAnyCut;
        PlayerCharacter.Instance.OnPickedSomething += PlayerCharacterOnPickedSomething;
        BaseCounter.OnAnyObjectPlaced += BaseCounterOnAnyObjectPlaced;
        TrashCounter.OnAnyObjectTrashed += TrashCounterOnAnyObjectTrashed;
    }

    private void TrashCounterOnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipsSO.trash, trashCounter.transform.position);
    }

    private void BaseCounterOnAnyObjectPlaced(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipsSO.objectDrop, baseCounter.transform.position);
    }

    private void PlayerCharacterOnPickedSomething(object sender, System.EventArgs e)
    {
        PlayerCharacter playerCharacter = PlayerCharacter.Instance;
        PlaySound(audioClipsSO.objectPickup, playerCharacter.transform.position);
    }

    private void CuttingCounterOnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManagerOnRecipeFailed(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipsSO.deliveryFail, deliveryCounter.transform.position);
    }

    private void DeliveryManagerOnRecipeSuccess(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipsSO.deliverySuccess, deliveryCounter.transform.position);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)],position,volume);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    public void playFootstepsSound(Vector3 positon, float volume = 1f)
    {
        PlaySound(audioClipsSO.footstep,positon, volume);
    }

public void PlayWarningSound(Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipsSO.warning,position,volume);
    }
}
