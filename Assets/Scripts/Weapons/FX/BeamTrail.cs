using System;
using System.Collections;
using UnityEngine;

public class BeamTrail : MonoBehaviour
{

    Transform origin;
    Transform destination;

    Action weaponContactCallback;
    public Timing beamDistanceTimer;
    public BeamAttributes beamAttributes;
    bool initialized = false;
    bool hitDetection = false;

    public LineRenderer line;

    public float afterGlowTime = 4;
    public MaterialPropertyBlock matBlock;

    [GradientUsage(true)]
    public Gradient gradient;

    float maxRange = 10;
    bool noHit = false;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        matBlock = new MaterialPropertyBlock();
    }

    // Update is called once per frame
    void Update()
    {
        if (initialized)
        {
            var distanceRatio = beamAttributes.beamDistanceCurve.Evaluate(beamDistanceTimer.GetProgressClamped);

            Vector3 position = Vector3.zero;
            if (origin == null)
            {
                position = line.GetPosition(1);
                distanceRatio = 1;
            }
            else
            {
                position = origin.position;
            }

            line.SetPosition(0, position);

            var headingAndDistance = (destination.position - position);
            var direction = headingAndDistance.normalized 
                * Mathf.Clamp(headingAndDistance.magnitude,
                0,
                maxRange)
                * distanceRatio;
            line.SetPosition(1, position + direction);
            UpdateOpacity();
            if (beamDistanceTimer.Completed())
            {
                Destroy(gameObject);
            }
            else
            {
                if (!noHit && !hitDetection && distanceRatio >= 1f)
                {
                    hitDetection = true;
                    //Debug.Log("Beam fire struck hull!");
                    weaponContactCallback.Invoke(); // determine accuracy if needed.
                }
            }
        }
    }


    public void UpdateOpacity()
    {
        var opacity = beamAttributes.beamOpacity.Evaluate(beamDistanceTimer.GetProgressClamped);
        matBlock.SetFloat("_Fade_overall", opacity);

        line.SetPropertyBlock(matBlock);
    }

    public void FireBegin(Action callback,
        Transform origin,
        Transform destination, float range)
    {
        weaponContactCallback = callback;
        this.origin = origin;
        this.destination = destination;

        beamDistanceTimer = new Timing();
        var beamDistance = Vector3.Distance(origin.position, destination.position);
        if (beamDistance > range)
        {
            noHit = true;
            beamDistance = range;
        }

        beamDistanceTimer.duration = beamDistance / beamAttributes.beamTravelSpeed + afterGlowTime;

        beamDistanceTimer.Init();
        Debug.Log("time for beam" + beamDistanceTimer.duration);

        transform.position = origin.position;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);

        initialized = true;
        maxRange = range;
    }
}

[Serializable]
public class BeamAttributes
{
    public AnimationCurve beamDistanceCurve;
    public float beamTravelSpeed = 100f;
    public AnimationCurve beamOpacity;
}