using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    public GameObject planningBar;

    public GameObject prevBtn;
    public GameObject nextBtn;

    public GameObject moveBtn;
    public GameObject fireWeapon;
    public GameObject endTurnBtn;

    public GameObject confirmNavBtn;

    // todo weapons.
    public GameObject disruptor;
    public GameObject laser;
    public GameObject missile;
    public GameObject torpedo;
    public GameObject cancleWeps;


    // Start is called before the first frame update
    void Start()
    {
        //SelectShip(false);
        planningBar.SetActive(false);  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectShip(bool isPlayer)
    {
        planningBar.SetActive(isPlayer);
        BringBackMainMenu();
    }

    public void ActivateMove(bool active)
    {
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
        fireWeapon.SetActive(true);
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
    }

    public void HideEverything()
    {
        prevBtn.SetActive(false);
        nextBtn.SetActive(false);

        moveBtn.SetActive(false);
        fireWeapon.SetActive(false);
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
