using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Weapon
{
    public bool firedThisRound = false;

    public BeamTrail beamTrailSpawn;

    public float damage = 10;
    public float armorPiercing = .25f;
    public float reactorDamageRatio = .5f;


    public float range = 50f;

    public override bool CanFireThisRound()
    {
        return !firedThisRound;
    }

    public override void BeginFiringQueue()
    {
        firedThisRound = true;
    }

    public override void FireWeapon(Ship target)
    {
        if (!firedThisRound)
        {
            var beam = Instantiate(beamTrailSpawn);
            beam.FireBegin(() => { 
                Debug.Log("damage callback"); 
                DamagePlayer(target); 
            }, transform, target.transform, range);
        }
    }

    void DamagePlayer(Ship target)
    {
        float damageToHull = damage; // TODO: check if hit armor
        target.DealDamage(damageToHull, damageToHull * reactorDamageRatio);
        // target.shipHealth.TakeDamage(damageToHull);
        //target.reactorHealth.TakeDamage(damageToHull * reactorDamageRatio);
    }

    public override void StartNewRound()
    {
        firedThisRound = false;
    }
}
