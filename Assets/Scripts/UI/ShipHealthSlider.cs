using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipHealthSlider : MonoBehaviour
{
    public Slider healthSlider;

    public Color friendlyHealthBarColor;

    public Color enemyHealthBarColor;

    public Image barColor;

    public void Initialize(bool isEnemy)
    {
        if(isEnemy)
        {
            barColor.color = enemyHealthBarColor;
        }
        else
        {
            barColor.color = friendlyHealthBarColor;
        }
    }
}
