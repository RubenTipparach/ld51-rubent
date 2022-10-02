using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class ManueverTimeline : MonoBehaviour
{
    public TimeMarker[] textNumbers;
    public Color defaultColor;
    public Color selectedColor;

    public TimesliceSelection timesliceSelection;
    public Slider timelineSlider;
    public FiringMarkerButton firingMarkerButton;
    public void SetHighlight(int i)
    {
        ClearHighlight();
        textNumbers[i].textMarker.color = selectedColor;
        GameManager.Instance.highlightedTimeSecond = i;

        if (i == 10)
        {
            GameManager.Instance.uiController.fireWeapon.interactable = false;
        }
        else
        {
            // update firing button if selectred index is not 10
            // and has or doesnt have weapons assigned.
            if (GameManager.Instance
                .selectedShip != null)
            {
                firingMarkerButton.SetFiringState(GameManager.Instance
                    .selectedShip.firingSolutiion.fireCommand[i].Count > 0);
            }

            GameManager.Instance.uiController.fireWeapon.interactable = true;
        }        
    }

    public void RunTimeline(bool running)
    {
        if (running)
        {
            timesliceSelection.gameObject.SetActive(false);
            timelineSlider.gameObject.SetActive(true);
        }
        else
        {
            timesliceSelection.gameObject.SetActive(true);
            timelineSlider.gameObject.SetActive(false);
        }
    }

    public void ClearHighlight()
    {
        for(int i = 0; i < textNumbers.Length; i++)
        {
            textNumbers[i].textMarker.color = defaultColor;
        }
    }

    public void LoadNewMarkers()
    {
        ClearWeaponMarkers();
        if (GameManager.Instance.selectedShip != null)
        {
            for (int i = 0; i < textNumbers.Length - 1; i++)
            {
                var firingCommands = GameManager.Instance.selectedShip.firingSolutiion.fireCommand;
                textNumbers[i].weaponMarker.enabled = firingCommands[i].Count > 0;
            }
        }
    }

    public void ClearWeaponMarkers()
    {
        for (int i = 0; i < textNumbers.Length - 1; i++)
        {
            textNumbers[i].weaponMarker.enabled = false;
        }
    }

    public void AddOrRemoveFiringMarker(int index, bool remove)
    {
        textNumbers[index].weaponMarker.enabled = !remove;

        if (remove)
        {
            GameManager.Instance.selectedShip.ClearSecond(index);
            textNumbers[index].weaponMarker.enabled = false;
        }
        else
        {
            GameManager.Instance.selectedShip.QueueWeaponFire(index);
            textNumbers[index].weaponMarker.enabled = true;
        }
    }

    public void ClearWepMarkers()
    {
        foreach(var wep in textNumbers)
        {
            if (wep.weaponMarker != null)
                wep.weaponMarker.enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
