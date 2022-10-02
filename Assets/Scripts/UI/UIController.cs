using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public GameObject planningBar;

    public GameObject prevBtn;
    public GameObject nextBtn;

    public GameObject moveBtn;
    public Button fireWeapon;
    public GameObject endTurnBtn;

    public GameObject confirmNavBtn;

    // todo weapons.
    public GameObject disruptor;
    public GameObject laser;
    public GameObject missile;
    public GameObject torpedo;
    public GameObject cancleWeps;

    public List<ShipHealthSlider> shipHealthSliders;

    public ShipHealthSlider templateHealthSlider;

    public float sliderYOffset = 20f;
    // Start is called before the first frame update
    void Start()
    {
        //SelectShip(false);
        planningBar.SetActive(false);

        foreach (var ship in GameManager.Instance.allShips)
        {
            var hSlider = Instantiate(templateHealthSlider, transform);
            ship.shipHealthSlider = hSlider;
            ship.shipHealthSlider.Initialize(ship.isPlayer);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var ship in GameManager.Instance.allShips)
        {
            var isVisible = IsVisible(Camera.main, ship.gameObject);
            if (isVisible)
            {
                ship.shipHealthSlider.gameObject.SetActive(true);

                RectTransform CanvasRect = GetComponent<RectTransform>();

                //then you calculate the position of the UI element
                //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.

                Vector2 viewportPosition = Camera.main.WorldToViewportPoint(ship.transform.position);
                Vector2 screenPosition = new Vector2(
                ((viewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
                ((viewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)))
                     + new Vector2(0, sliderYOffset);

                ((RectTransform)ship.shipHealthSlider.healthSlider.transform).anchoredPosition = screenPosition;
            }
            else
            {
                ship.shipHealthSlider.gameObject.SetActive(false);
            }
        }
    }

    private bool IsVisible(Camera c, GameObject target)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(c);
        var point = target.transform.position;

        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(point) < 0)
            {
                return false;
            }
        }
        return true;
    }

    public void SelectShip(bool isPlayer)
    {
        planningBar.SetActive(isPlayer);
        BringBackMainMenu();
    }

    public void ActivateMove(bool active)
    {
        GameManager.Instance.navController.shipSelected = GameManager.Instance.selectedShip;
        GameManager.Instance.navController.ActivateController(active);
        if(active)
        {
            HideEverything();
            confirmNavBtn.SetActive(active);
        }
        else
        {
            HideEverything();
        }
    }

    void BringBackMainMenu()
    {
        HideEverything();

        prevBtn.SetActive(true);
        nextBtn.SetActive(true);

        moveBtn.SetActive(true);
        fireWeapon.gameObject.SetActive(true);
        endTurnBtn.SetActive(true);
    }

    public void ActivateWeapons(bool active)
    {
        
    }

    public void ConfirmManuever()
    {
        GameManager.Instance.navController.Confirm();
        BringBackMainMenu();
    }

    public void EndTurn()
    {
        GameManager.Instance.EndTurn();
        planningBar.SetActive(false);
    }

    public void HideEverything()
    {
        prevBtn.SetActive(false);
        nextBtn.SetActive(false);

        moveBtn.SetActive(false);
        fireWeapon.gameObject.SetActive(false);
        endTurnBtn.SetActive(false);

        confirmNavBtn.SetActive(false);

        //disruptor.SetActive(false);
        //laser.SetActive(false);
        //missile.SetActive(false);
        //torpedo.SetActive(false);
        //cancleWeps.SetActive(false);
    }
    public void NextShip(bool previous  = false)
    {
        if(previous)
        {
            // call game manager to get array of active player ships.
        }
        else
        {

        }
    }
}
