using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanicSoundController : MonoBehaviour
{
    public AudioSource panicSound;
    public float fadeTime = 30;
    private float elapsedTime = 0.0f;
    private bool playSound; 
    public float delayedTime;
    // Start is called before the first frame update
    void Start()
    {
        panicSound = GetComponent<AudioSource>();
        playSound = false;
        elapsedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (playSound)
        {
            elapsedTime += Time.deltaTime;
            if (!panicSound.isPlaying && elapsedTime >= delayedTime)
            {
                    panicSound.Play();
            }
            if (panicSound.isPlaying && elapsedTime < fadeTime)
            {
                //Debug.Log(panicSound.volume);
                panicSound.volume = 1 - elapsedTime / fadeTime;
                elapsedTime += 0.02f;

            }
            else if(elapsedTime > fadeTime) {
                playSound = false;
                elapsedTime = 0;
                panicSound.Stop();
            }
        }
    }


    public void PlaySound()
    {
        playSound = true;
    }
}
