using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class Ship : MonoBehaviour
{
    public string shipName = "";

    public float maxRotationArc = 90f; // 9 degrees/s

    public float maxMovementDistance = 20f; // 2 km/s

    public bool movementOrderedChange = false;

    public Maneuver maneuverSelected;

    public MeshFilter shipGhost;

    public bool isPlayer = false;

    public GameObject moveDestViz;

    Vector3 origin;
    Vector3 destination;

    Quaternion startRotation;

    public HealthStats shipHealth;

    public HealthStats reactorHealth;

    public Weapon[] weapons;

    public FiringSolutiion firingSolutiion;

    public ShipHealthSlider shipHealthSlider;

    public Explosion explosionImpact;
    public Explosion shipDestroyed;
    public float impactExpDelay = .3f;
    public void DealDamage(float hullDamage, float reactorDamage, Vector3? hullImpactPoint = null)
    {
        shipHealth.TakeDamage(hullDamage);

        if(hullImpactPoint != null)
        {
            // blow up in that spot
        }
        else
        {
            StartCoroutine(DelayExplosion());
        }

        //reactorHealth.TakeDamage(reactorDamage);

        //healthSprite.
        shipHealthSlider.healthSlider.value = shipHealth.Percent;

        if(reactorHealth.IsDead)
        {
            //ship is disabled.
        }

        if(shipHealth.IsDead)
        {
            // ship explodes.
            Instantiate(shipDestroyed, transform.position, Quaternion.identity);
            Destroy(shipHealthSlider.gameObject);
            StartCoroutine(DestroyShipCoroutine());
        }
    }
    IEnumerator DelayExplosion()
    {
        yield return new WaitForSeconds(impactExpDelay);
        Instantiate(explosionImpact, transform.position, Quaternion.identity);
    }

    IEnumerator DestroyShipCoroutine()
    {
        yield return new WaitForEndOfFrame();
        GameManager.Instance.allShips.Remove(this); //fuck it, lets blow it up!

        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        if (string.IsNullOrEmpty(shipName))
        {
            shipName = transform.name;
        }

        firingSolutiion = new FiringSolutiion();
        firingSolutiion.Initialize();
        shipHealth.Init();
        reactorHealth.Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveDestViz.activeInHierarchy)
        {
            moveDestViz.transform.position = destination;
        }
    }

    public void StartOfNewTurn()
    {
        destination = transform.position + maneuverSelected.destinationLocalOffset;

        //decide if orientation facing enemy has to be updated.
        if (!movementOrderedChange)
        {
            if (maneuverSelected.targetSelected != null)
            {
                maneuverSelected.targetOrientation = Quaternion.LookRotation((
                    maneuverSelected.targetSelected.transform.position -
                     destination).normalized);
            }
        }

        if (isPlayer)
        {
            ShowMovementPlan();
        }

        //clear flags.
        //firingSolutiion.Clear();
        movementOrderedChange = false;
    }

    public void ConfirmMove(Vector3 dest, Quaternion ort, float offset, Ship target)
    {
        // this is if the user didnt issue any changes the ship will stay oriented to target.
        var orientation = ort;

        maneuverSelected.ConfirmMove(dest, orientation, offset, target);

        origin = transform.position;
        startRotation = transform.rotation;
        destination = transform.position + maneuverSelected.destinationLocalOffset;

        ShowMovementPlan();
    }


    public void ShowMovementPlan()
    {
        moveDestViz.transform.position = destination;
        moveDestViz.transform.rotation = maneuverSelected.targetOrientation;
        moveDestViz.SetActive(true);
    }
    public void EndTurn()
    {

        if (!maneuverSelected.initialDestSet)
        {
            maneuverSelected.Initialize(this);
        }

        startRotation = transform.rotation;
        origin = transform.position;
        destination = origin + maneuverSelected.destinationLocalOffset;

        //auto fire anything queued at the 0th second.
        CheckAndFireWeapons(0);
    }

    public void UpdateShipPositionAndRotation(float percent)
    {
        transform.position = Vector3.Lerp(origin, destination, percent);
        transform.rotation = Quaternion.Lerp(startRotation, maneuverSelected.targetOrientation, percent);
    }

    public void QueueWeaponFire(int timeSecond,
        bool remove = false,
        int wepIndex = 0)
    {

        firingSolutiion.QueueWeaponFire(timeSecond,
            weapons[wepIndex]);
    }

    public void ClearSecond(int timeSecond)
    {
        firingSolutiion.ClearSecond(timeSecond);
    }

    public void CheckAndFireWeapons(int timeSecond)
    {
        firingSolutiion.FireWeapons(timeSecond);
    }


    public float waitToCheckCollision = .2f;
    public float defaultCollisionDamage = 20f;
    bool collisionCheck = false;

    public LayerMask layerMask;

    //easy to just hack the physics masking table for now
    void OnCollisionStay(Collision other)
    {
        if (!collisionCheck 
            && 
            (layerMask == (layerMask | 1 << other.transform.gameObject.layer)))
        {
            StartCoroutine(CollisionDamage());
        }
    }

    IEnumerator CollisionDamage()
    {
        collisionCheck = true;

        if (this != null && !shipHealth.IsDead)
        {
            DealDamage(defaultCollisionDamage, 0);
        }

        yield return new WaitForSeconds(waitToCheckCollision);

        collisionCheck = false;
    }
}


[Serializable]
public class Maneuver
{
    public Vector3 destinationLocalOffset;
    public Quaternion targetOrientation;
    public float offsetElevationTarget = 0;
    public bool initialDestSet = false;
    public Ship targetSelected;
    public void Initialize(Ship ship)
    {
        initialDestSet = true;
        targetOrientation = ship.transform.rotation;
        destinationLocalOffset = ship.transform.forward * ship.maxMovementDistance / 4f;
    }

    public void ConfirmMove(Vector3 dest, Quaternion ort, float offset, Ship target)
    {
        targetOrientation = ort;
        destinationLocalOffset = dest;
        offsetElevationTarget = offset;
        targetSelected = target;
    }

}

public class FiringSolutiion
{
    public Dictionary<int, List<FireCommand>> fireCommand;
    public Ship targetFiring;

    public void Initialize()
    {
        fireCommand = new Dictionary<int, List<FireCommand>>();

        // number 10 should not be able to fire.
        for (int i = 0; i < 10; i++)
        {
            fireCommand.Add(i, new List<FireCommand>());
        }
    }

    public void SetWeaponsToTarget(Ship target)
    {
        targetFiring = target;
    }

    public void FireWeapons(int timeSecond)
    {
        if (targetFiring == null) { return; }

        foreach (var weps in fireCommand[timeSecond])
        {
            weps.weaponQueued.FireWeapon(targetFiring);
        }
    }

    public void QueueWeaponFire(int timeSecond, Weapon weapon)
    {
        fireCommand[timeSecond].Add(new FireCommand(weapon));
    }
    public void ClearSecond(int timeSecond)
    {
        fireCommand[timeSecond].Clear();
    }

    public void Clear()
    {
        // clear all weapons queue.
        for (int i = 0; i < 10; i++)
        {
            fireCommand[i].Clear();
        }
    }



}

[Serializable]
public class FireCommand
{
    public FireCommand(Weapon weapon)
    {
        weaponQueued = weapon;
    }

    public Weapon weaponQueued;
}

public enum WeaponType
{
    Disruptor,
    Laser,
    Missile,
    Torpedo
}