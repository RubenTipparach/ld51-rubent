using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public Timing timer;

    bool simulationRunning = false;

    Ship[] allShips;

    public HealthStats shipHealth;

    public HealthStats reactorHealth;

    private void Awake()
    {
        gameManager = this;
        gameInput = new GameInput();
    }

    // Start is called before the first frame update
    void Start()
    {
        orbitShipCamera.SetupCamera(orbitShipCamera.target, orbitShipCamera.mainCamera.transform);

        
        //Time.timeScale = 0;

    }

    IEnumerator lateStart()
    {
        yield return new WaitForEndOfFrame();
    }

    // Update is called once per frame
    void Update()
    {
        gameInput.UpdateInput();

        if (simulationRunning)
        {

            foreach (var s in allShips)
            {
                s.UpdateShipPositionAndRotation(timer.GetProgressClamped);
            }

            if (timer.GetProgressClamped == 1f)
            {
                simulationRunning = false;
            }
        }

        bool uiRaycastBlock = EventSystem.current.IsPointerOverGameObject();

        //Debug.Log(gameInput.MouseDelta);
        if (Input.GetMouseButtonDown(0) && !navController.navModeActive)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 10000))
            {
                if (hit.rigidbody != null && hit.rigidbody.GetComponent<Ship>() != null)
                {

                    var ship = hit.rigidbody.GetComponent<Ship>();

                    Debug.Log(ship.shipName + " ship selected");
                    //var f = Time.deltaTime;
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
                    if (selectedShip.isPlayer && simulationRunning == false)
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

        bool selectedShipClick = false;

        if (Input.GetMouseButton(0) && navController.navModeActive)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 10000))
            {
                if (hit.rigidbody != null && hit.rigidbody.GetComponent<Ship>() != null)
                {
                    var target = hit.rigidbody.GetComponent<Ship>();
                    if (selectedShip.isPlayer && simulationRunning == false && target != selectedShip)
                    {
                        var orientation = Quaternion.LookRotation((hit.transform.position -
                            navController.shipPositionDestination.transform.position).normalized);

                        navController.UpdateOrientationPosition(orientation);
                        selectedShipClick = true;
                    }
                }
            }
        }

        if (navController.navModeActive && !selectedShipClick)
        {
            if (Input.GetMouseButtonDown(0))
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

    public void EndTurn()
    {
        UpdateEnemyLogic();
        simulationRunning = true;
        timer.StartTimerAt(0);

        allShips = FindObjectsOfType<Ship>();
        foreach(var s in allShips)
        {
            s.EndTurn();
        }
    }


    public void UpdateEnemyLogic()
    {
        var enemyShips = FindObjectsOfType<Ship>().Where(p => !p.isPlayer);

        foreach(var es in enemyShips)
        {
            if(!es.maneuverSelected.initialDestSet)
            {
                es.maneuverSelected.Initialize(es);
            }

            // else randomly rotate (to face strongest player or weak player by prob.)
            // and move to location close to the player.
            // fire weapons based on probability for each 10 rounds per turn.
        }
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
