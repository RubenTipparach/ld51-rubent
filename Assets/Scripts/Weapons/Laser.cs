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
            beam.FireBegin(() => DamagePlayer(target), transform, target.transform);
        }
    }

    void DamagePlayer(Ship target)
    {
        float damageToHull = damage; // TODO: check if hit armor
        target.shipHealth.TakeDamage(damageToHull);
        target.reactorHealth.TakeDamage(damageToHull * reactorDamageRatio);
    }

    public override void StartNewRound()
    {
        firedThisRound = false;
    }
}
