using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance => gameManager;
    private static GameManager gameManager;

    //public UIManager uiManager;
    public OrbitShipCamera orbitShipCamera;
    public CubeController targetController;
    public Ship selectedShip;

    public LayerMask shipMask;
    public LayerMask interactive;

    public UIController uiController;

    // Steps to manuever:
    public NavigationController navController;

    public WeaponsCommandController weaponsCommandController;


    private void Awake()
    {
        gameManager = this;
        gameInput = new GameInput();
    }

    // Start is called before the first frame update
    void Start()
    {
        orbitShipCamera.SetupCamera(orbitShipCamera.target, orbitShipCamera.mainCamera.transform);

    }

    // Update is called once per frame
    void Update()
    {
        gameInput.UpdateInput();
        //Debug.Log(gameInput.MouseDelta);
        bool selectedShipClick = false;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 10000))
            {
                if (hit.rigidbody != null && hit.rigidbody.GetComponent<Ship>() != null)
                {
                    
                    var ship = hit.rigidbody.GetComponent<Ship>();

                    Debug.Log(ship.shipName + " ship selected");

                    //targetController.transform.position = ship.transform.position;
                    targetController.followObj = ship.transform;

                    selectedShip = ship;
                    // todo: display ui data.
                    //if (selectedShip.isPlayer)
                    //{
                    //    navController.SelectShip(ship);
                    //}
                    //else
                    //{
                    //    navController.ActivateController(false);
                    //}
                    if(selectedShip.isPlayer)
                    {
                        navController.shipSelected = ship;
                    }
                    else
                    {
                        navController.CancelManuever();
                        uiController.HideEverything();
                        //cancel weapons stuff.
                    }

                    uiController.SelectShip(selectedShip.isPlayer);

                }
            }
        }
        bool uiRaycastBlock = EventSystem.current.IsPointerOverGameObject();

        
        if (navController.navModeActive)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 10000))
                {
                    if (hit.transform.GetComponent<ElevationWidget>() != null)
                    {
                        navController.elevationWidget.interacting = true;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                navController.elevationWidget.interacting = false;
                navController.offsetElevation =
                    navController.shipPositionDestination.transform.position.y -
                    navController.shipSelected.transform.position.y;
            }

            if (!uiRaycastBlock 
                && !navController.elevationWidget.interacting
                && Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                Plane plane = new Plane(Vector3.up,
                    -navController.shipSelected.transform.position.y
                    - navController.offsetElevation);

                if (plane.Raycast(ray, out float hitDistance))
                {
                    var des = ray.GetPoint(hitDistance);
                    navController.UpdateDestinationPosition(des);
                }
            }
        }
    }

    internal void EndTurn()
    {
        throw new NotImplementedException();
    }

    public static GameInput GameInput
    {
        get
        {
            return gameInput;
        }
    }


    private static GameInput gameInput = null;

}
