using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NavigationController : MonoBehaviour
{
    public ShipHolo shipPositionHighlight;

    public ShipHolo shipPositionDestination;

    public ShipHolo shipPositionPreview;

    public GameObject navPlane;

    public GameObject[] intervalMarkers;

    public LineRenderer navLine;
    public LineRenderer elevationLine;

    public float offsetElevation;

    public bool navModeActive = false;

    public Ship shipSelected;

    public ElevationWidget elevationWidget;

    public Ship rotateToTarget;

    public LineRenderer attackLine;

    public void SetElevationOffset(float value)
    {
        offsetElevation = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        ActivateController(false);
        offsetElevation = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(attackLine.enabled)
        {
            if(shipSelected.firingSolutiion.targetFiring != null)
            {

            var targetPosition = shipSelected.firingSolutiion.targetFiring;
            attackLine.SetPosition(0, shipPositionDestination.transform.position);
            attackLine.SetPosition(1, targetPosition.transform.position);
            }
            else {
                shipSelected.firingSolutiion.targetFiring = null;
            }
        }
    }

    public void UpdateOrientationPosition(Quaternion destination)
    {
        shipPositionDestination.transform.rotation = destination;
    }

    public void UpdateDestinationPosition(Vector3 destination)
    {
        var shipStart = shipSelected.transform.position;

        var heading = destination - shipStart;

        //check if max distance is reached
        heading = heading.magnitude > shipSelected.maxMovementDistance ?
            heading.normalized * shipSelected.maxMovementDistance
            :
            heading;

        destination = shipStart + heading;

        // set points
        shipPositionDestination.transform.position = destination;
        navLine.SetPosition(0, shipStart);
        navLine.SetPosition(1, destination);

        for (int i = 0; i < intervalMarkers.Length; i++)
        {
            var distance = heading *  ((i +1) / 10f) + shipStart;
            intervalMarkers[i].transform.position = distance;
        }


        if (Input.GetKey(KeyCode.LeftControl))
        {
            UpdateOrientationPosition(Quaternion.LookRotation(heading.normalized));

            // you don't have to shoot at them I guess...
            rotateToTarget = null;
        }
    }

    public void SelectShip(Ship ship)
    {
        shipSelected = ship;
    }

    public void ActivateController(bool active)
    {
        navLine.enabled = active;
        elevationLine.enabled = active;

        shipPositionDestination.enabled = active;
        attackLine.enabled = false;

        shipPositionHighlight.enabled = false;
        shipPositionPreview.enabled = false; // only shows when you hover ui.

        navModeActive = active;
        foreach (var m in intervalMarkers) { m.SetActive(active); }

        if (active)
        {
            rotateToTarget = shipSelected.maneuverSelected.targetSelected;

            if (!shipSelected.maneuverSelected.initialDestSet)
            {
                shipSelected.maneuverSelected.Initialize(shipSelected);
                UpdateDestinationPosition(shipSelected.maneuverSelected.destinationLocalOffset + shipSelected.transform.position);
            }

            if(shipSelected.firingSolutiion.targetFiring != null)
            {
                attackLine.enabled = true;
            }

            SetShipModel(shipSelected.shipGhost.sharedMesh);
            shipSelected.moveDestViz.SetActive(false);

            shipPositionDestination.transform.rotation = shipSelected.maneuverSelected.targetOrientation;
            //shipPositionDestination.transform.position = shipSelected.maneuverSelected.destinationLocalOffset + shipSelected.transform.position;
            UpdateDestinationPosition(shipSelected.maneuverSelected.destinationLocalOffset + shipSelected.transform.position);
            offsetElevation = shipSelected.maneuverSelected.offsetElevationTarget;
        }
        else
        {
            rotateToTarget = null;
        }
    }

    private void SetShipModel(Mesh shipMesh)
    {
        shipPositionHighlight.meshFilter.mesh = shipMesh;
        shipPositionDestination.meshFilter.mesh = shipMesh;
        shipPositionPreview.meshFilter.mesh = shipMesh; 
    }

    public void ShowPreviewInt(int index)
    {
        shipPositionPreview.enabled = true;

        if (navModeActive)
        {
            shipPositionPreview.transform.position = intervalMarkers[index].transform.position;
            // todo change preview orientation
        }
    }

    public void OnHoverOff()
    {
        shipPositionPreview.enabled = false;
    }

    public void Confirm()
    {
        var orientation = shipPositionDestination.transform.rotation;

        //if (rotateToTarget != null)
        //{
        //    orientation = Quaternion.LookRotation((rotateToTarget.transform.position - shipPositionDestination.transform.position).normalized);
        //}

        shipSelected.movementOrderedChange = true;
        shipSelected.ConfirmMove(shipPositionDestination.transform.position - shipSelected.transform.position,
            orientation,
            offsetElevation, rotateToTarget);

        ActivateController(false);
    }

    public void CancelManuever()
    {
        ActivateController(false);
    }

    public void TargetShip(Ship target)
    {
        var orientation = Quaternion.LookRotation((target.transform.position -
                    shipPositionDestination.transform.position).normalized);
        rotateToTarget = target;
        UpdateOrientationPosition(orientation);

        if (shipSelected.firingSolutiion.targetFiring != null)
        {
            attackLine.enabled = true;
        }
    }
}
