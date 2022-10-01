using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ElevationWidget : MonoBehaviour
{

    public NavigationController navController;

    public float sensitivity = 4;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(interacting)
        {
            var yOffset = GameManager.GameInput.MouseDelta.y;

            navController.offsetElevation += yOffset * Time.unscaledDeltaTime * sensitivity;

            var position = new Vector3(navController.shipPositionDestination.transform.position.x,
                navController.shipSelected.transform.position.y,
                navController.shipPositionDestination.transform.position.z);

            navController.UpdateDestinationPosition(position + Vector3.up*navController.offsetElevation);
        }
    }

    public bool interacting = false;
}
