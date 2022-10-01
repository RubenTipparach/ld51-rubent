using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CubeController : MonoBehaviour
{

    public float moveSpeed = 10;

    float smoothVelocity;

    public float smoothTime = 10;

    float internalSpeed = 0;

    public Camera cam;

    public float lerpSpeed = 10;

    (KeyCode, Vector3)[] controls = new[]{
            (KeyCode.W, Vector3.forward),
            (KeyCode.S, Vector3.back),
            (KeyCode.A, Vector3.left),
            (KeyCode.D, Vector3.right),
            (KeyCode.R, Vector3.up),
            (KeyCode.F, Vector3.down),
    };
    Vector3 directionMove;

    public Transform followObj;

    private void Start()
    {
        //var camDirection = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1));
        //transform.forward = camDirection;
    }

    void Update()
    {
        bool smoothStart = false;

        foreach (var control in controls)
        {
            if (Input.GetKey(control.Item1))
            {
                smoothStart = true;
                directionMove += control.Item2;
                followObj = null;
            }
        }


        if (followObj == null)
        {
            directionMove = directionMove.normalized;

            var traget = smoothStart ? moveSpeed : 0f;

            internalSpeed = Mathf.SmoothDamp(internalSpeed, traget, ref smoothVelocity, smoothTime);

            var camDirection = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1));
            var direction = Quaternion.LookRotation(camDirection) * directionMove;
            transform.Translate(direction * Time.deltaTime * internalSpeed, Space.World);
        }
        else
        {
            var snapThresh = .1f;
            if(Vector3.Distance(transform.position, followObj.position) > snapThresh)
            {
                transform.position = Vector3.Lerp(transform.position, followObj.position, lerpSpeed * Time.deltaTime);
            }else
            {
                transform.position = followObj.position; 
            }
            internalSpeed = 0;
        }
    }
}
