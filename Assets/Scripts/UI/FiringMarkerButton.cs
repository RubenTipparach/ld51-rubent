using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class FiringMarkerButton : MonoBehaviour
{

    public bool isInFiringState = true;

    public TextMeshProUGUI displayText;

    private void Start()
    {
        displayText.text = "Attack";
    }

    public void SetFiringState(bool firing)
    {
        if (firing)
        {
            displayText.text = "Cancel";
            isInFiringState = false;
        }
        else
        {
            displayText.text = "Attack";
            isInFiringState = true;
        }

    }
    public void FireAtMark()
    {
        var highlightedIndex = GameManager.Instance.highlightedTimeSecond;


        if (isInFiringState)
        {
            //add marker -- can I Add?
            // check weapon index 0
            if (GameManager.Instance.selectedShip.CanFireShots(0))
            {
                GameManager.Instance.manueverTimeline
                    .AddOrRemoveFiringMarker(highlightedIndex, false);
                SetFiringState(isInFiringState);
            }

        }
        else
        {
            // remove marker
            GameManager.Instance.manueverTimeline
                .AddOrRemoveFiringMarker(highlightedIndex, true);
            SetFiringState(isInFiringState);
        }

    }
}
