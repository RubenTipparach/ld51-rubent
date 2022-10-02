using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    public Timing cleanupTiming;

    float pausedExplosionAtTime = 0;

    public ParticleSystem explosionParticles;


    public void Pause()
    {
        pausedExplosionAtTime = cleanupTiming.Remaining;
        explosionParticles.Pause();
    }

    public void Resume()
    {
        cleanupTiming.duration = pausedExplosionAtTime;
        cleanupTiming.Init();
        explosionParticles.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        cleanupTiming.Init();
        cleanupTiming.StartTimerAt(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (cleanupTiming.Completed())
        {
            Destroy(gameObject);
        }
    }
}
