using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Weapon
{
    void StartNewRound();

    void FireWeapon(Ship target);

    bool CanFireThisRound();

    void BeginFiringQueue();
}
