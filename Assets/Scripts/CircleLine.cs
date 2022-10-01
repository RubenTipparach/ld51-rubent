using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleLine : MonoBehaviour
{
    public LineRenderer circleLine;

    public float radius = 5;

    public Transform trackingTarget;

    int lineSegements = 16;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(circleLine != null)
        {
            var plane = GameManager.Instance.orbitShipCamera.target;
            var planePosition = Vector3.Scale(trackingTarget.position, new Vector3(1, 0, 1));

            circleLine.SetPosition(0, trackingTarget.position);
            circleLine.SetPosition(1, planePosition);

            var lookDir = Vector3.Scale(trackingTarget.forward, new Vector3(1, 0, 1)).normalized;

            for (int i = 0; i < lineSegements; i++)
            {
                var heading = 
                    Quaternion.Euler(0, ((float)i/lineSegements) * 360f, 0) *
                    lookDir * (radius);

                var position = heading + planePosition;

                //Debug.DrawLine(trackingTarget.transform.position, position, Color.white);

                circleLine.SetPosition(2 + i, position);

            }

            circleLine.SetPosition(lineSegements + 2, //Quaternion.Euler(0, (0 / lineSegements) * 360f, 0)*
                 lookDir * (radius) + planePosition);
        }
    }
}
