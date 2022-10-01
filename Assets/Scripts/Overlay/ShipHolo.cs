using System.Collections;
using UnityEngine;

public class ShipHolo : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    public GameObject elevationWidget;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        meshRenderer.enabled = true;

        if(elevationWidget != null)
        {
            elevationWidget.SetActive(true);
        }

    }

    private void OnDisable()
    {
        meshRenderer.enabled = false;
        if (elevationWidget != null)
        {
            elevationWidget.SetActive(false);
        }
    }
}
