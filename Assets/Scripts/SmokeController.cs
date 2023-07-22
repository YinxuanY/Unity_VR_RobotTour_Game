using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeController : MonoBehaviour
{
    ParticleSystem smoke;
    ParticleSystem.MainModule main;
    ParticleSystem.ShapeModule shape;
    ParticleSystem.EmissionModule emission;

    // Start is called before the first frame update
    void Start()
    {
        smoke = GetComponent<ParticleSystem>();
        main = smoke.main;
        shape = smoke.shape;
        emission = smoke.emission;
    }


    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad > 10 && Time.timeSinceLevelLoad < 14)
        {
            shape.radius = 3;
            main.maxParticles = 750;
            main.startSize = 3;
            emission.enabled = true;
        }

        if (Time.timeSinceLevelLoad > 14 && Time.timeSinceLevelLoad < 18)
        {
            shape.radius = 5;
            main.maxParticles = 1000;
            emission.rateOverTime = 200;
        }

        if (Time.timeSinceLevelLoad > 18 && Time.timeSinceLevelLoad < 22)
        {
            shape.radius = 10;
            main.maxParticles = 1500;
            emission.rateOverTime = 200;
        }

        if (Time.timeSinceLevelLoad > 22)
        {
            shape.radius = 15;
            main.maxParticles = 2500;
            emission.rateOverTime = 300;
        }
    }
}
