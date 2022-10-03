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
            // first do raycast to see if we hit a piece of armor.
            var layerMask = GameManager.Instance.shipMask;
            var dir = (target.transform.position - transform.position).normalized;
            var ray = new Ray(transform.position,dir);
            bool hitArmor = false;
            bool hitAsteroid = false;
            Debug.DrawLine(ray.origin, ray.direction*100f, Color.white, 10f);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, layerMask))
            {
                Debug.Log("laser hit object " + hit.collider.transform.name);
                if (hit.collider.transform.tag == "armor")
                {
                    hitArmor = true;
                }

                if (hit.rigidbody == null)
                {
                    Debug.Log("you shot a solid object!");
                    hitAsteroid = true;
                }
            }
            else
            {
                Debug.Log("found nothin");
            }


            beam.FireBegin(() => { 
                //Debug.Log("damage callback"); 
                DamagePlayer(target, hitArmor); 
            }, transform, target.transform, range, hitAsteroid);
        }
    }

    void DamagePlayer(Ship target, bool hitArmor)
    {


        float damageToHull = damage; // TODO: check if hit armor
        target.DealDamage(damageToHull, damageToHull * reactorDamageRatio, null, hitArmor);
        // target.shipHealth.TakeDamage(damageToHull);
        //target.reactorHealth.TakeDamage(damageToHull * reactorDamageRatio);
    }

    public override void StartNewRound()
    {
        firedThisRound = false;
    }
}
