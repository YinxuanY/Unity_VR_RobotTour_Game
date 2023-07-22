using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSoundController : MonoBehaviour
{
    public AudioSource fireSound;
    private float elapsedTime = 0.0f;
    private bool callOnce;
    public float delayedTime = 10;
    // Start is called before the first frame update
    void Start()
    {
        fireSound = GetComponent<AudioSource>();
        callOnce = true;
    }


    // Update is called once per frame
    void Update()
    {
        /*
        if (SimController.hasStarted)
        {
            elapsedTime += Time.deltaTime;
            if (callOnce && elapsedTime >= delayedTime)
            {
                elapsedTime = 0;
                fireSound.Play();
                callOnce = false;
            }
        }*/
    }
}
