using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundController : MonoBehaviour
{
    public AudioSource Sound;
    public float extendedTime;

    // Start is called before the first frame update
    void Start()
    {
        Sound.loop = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void playSound(bool play)
    {
        if (play)
        {
            Debug.Log("playing ambient");
            Sound.Play();
        }
        else
            Sound.Stop();
    }
}
