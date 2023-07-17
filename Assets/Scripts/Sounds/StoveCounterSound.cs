using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private AudioSource audioSource;
    private float warningSoundTimer;
    private bool playWarningSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounterOnStateChanged;
        stoveCounter.OnProgressChanged += StoveCounterOnProgressChanged;
    }

    private void StoveCounterOnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burnProgressAmount = 0.2f;
        playWarningSound = stoveCounter.IsFried() && e.progressNormalized > burnProgressAmount;
    }

    private void StoveCounterOnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool playSound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        if (playSound)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }

        
    }

    private void Update()
    {
        if (playWarningSound)
        {
            warningSoundTimer -= Time.deltaTime;
            if (warningSoundTimer <= 0f)
            {
                float warningSoundTimeMax = 0.1f;
                warningSoundTimer = warningSoundTimeMax;
                // Number is volume
                float volume = 1.2f;
                SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position, volume);
            }
        }
        
    }
}
