﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Ship : MonoBehaviour
{
    public string shipName = "";

    public float maxRotationArc = 90f; // 9 degrees/s

    public float maxMovementDistance = 20f; // 2 km/s

    public bool confirmedManuevers = false;

    public List<FireCommand> fireCommands;

    public Maneuver maneuverSelected;

    public MeshFilter shipGhost;

    public bool isPlayer = false;

    public GameObject moveDestViz;

    Vector3 origin;
    Vector3 destination;

    // Use this for initialization
    void Start()
    {
        if (string.IsNullOrEmpty(shipName))
        {
            shipName = transform.name;
        }

        fireCommands = new List<FireCommand>();
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
        confirmedManuevers = false;
        fireCommands.Clear();
    }

    public void ConfirmMove(Vector3 dest, Quaternion ort, float offset)
    {
        maneuverSelected.ConfirmMove(dest, ort, offset);
        
        origin = transform.position;
        destination = origin + dest;

        moveDestViz.transform.position = origin + dest;
        moveDestViz.transform.rotation = ort;
        moveDestViz.SetActive(true);
    }
}


[Serializable]
public class Maneuver
{
    public Vector3 destinationLocalOffset;
    public Quaternion targetOrientation;
    public float offsetElevationTarget = 0;
    public bool initialDestSet = false;

    public void Initialize(Ship ship)
    {
        initialDestSet = true;
        targetOrientation = ship.transform.rotation;
        destinationLocalOffset = ship.transform.forward * ship.maxMovementDistance / 4f;
    }

    public void ConfirmMove(Vector3 dest, Quaternion ort, float offset)
    {
        targetOrientation = ort;
        destinationLocalOffset = dest;
        offsetElevationTarget = offset;
    }
}

[Serializable]
public class FireCommand
{
    public WeaponType weaponType;
    int markStep = 0;
}

public enum WeaponType
{
    Disruptor,
    Laser,
    Missile,
    Torpedo
}