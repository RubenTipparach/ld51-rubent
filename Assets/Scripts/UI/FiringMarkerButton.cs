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
            GameManager.Instance.manueverTimeline
                .AddOrRemoveFiringMarker(highlightedIndex, false);
        }
        else
        {
            GameManager.Instance.manueverTimeline
                .AddOrRemoveFiringMarker(highlightedIndex, true);
        }

        SetFiringState(isInFiringState);
    }
}
