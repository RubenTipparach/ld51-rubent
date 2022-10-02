using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract void StartNewRound();

    public abstract void FireWeapon(Ship target);

    public abstract bool CanFireThisRound();

    public abstract void BeginFiringQueue();
}
