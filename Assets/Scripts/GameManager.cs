using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        if(Input.GetMouseButtonDown(0))
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
                }
            }
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
