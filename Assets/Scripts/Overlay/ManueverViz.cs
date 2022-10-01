using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManueverViz : MonoBehaviour
{
    // Start is called before the first frame update

    public Ship ship;

    public LineRenderer lineRenderer;

    public Vector3 destination;

    private void Update()
    {
        lineRenderer.SetPosition(0, ship.transform.position);
        lineRenderer.SetPosition(1, transform.position);

    }
}
