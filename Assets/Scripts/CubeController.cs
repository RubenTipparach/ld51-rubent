using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CubeController : MonoBehaviour
{

    (KeyCode, Vector3)[] controls = new[]{
            (KeyCode.W, Vector3.up),
            (KeyCode.S, Vector3.down),
            (KeyCode.A, Vector3.left),
            (KeyCode.D, Vector3.right),
        };


    void Update()
    {
        foreach (var control in controls)
        {
            if (Input.GetKey(control.Item1))
            {
                transform.Translate(control.Item2 * Time.deltaTime);
            }
        }
    }
}
