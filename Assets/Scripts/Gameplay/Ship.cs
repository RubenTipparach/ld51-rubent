using System.Collections;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public string shipName = "";

    // Use this for initialization
    void Start()
    {
        if (string.IsNullOrEmpty(shipName))
        {
            shipName = transform.name;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
