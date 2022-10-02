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
            line.SetPosition(0, origin.position);

            var direction = (destination.position - origin.position) * distanceRatio;
            line.SetPosition(1, origin.position + direction);
            UpdateOpacity();
            if (beamDistanceTimer.Completed())
            {
                Destroy(gameObject);
            }
            else
            {
                if (!hitDetection && distanceRatio >= 1f)
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

    public void FireBegin(Action callback, Transform origin, Transform destination)
    {
        weaponContactCallback = callback;
        this.origin = origin;
        this.destination = destination;

        beamDistanceTimer = new Timing();
        beamDistanceTimer.duration = Vector3.Distance(origin.position, destination.position) / beamAttributes.beamTravelSpeed + afterGlowTime;
        beamDistanceTimer.Init();
        Debug.Log("time for beam" + beamDistanceTimer.duration);

        transform.position = origin.position;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);

        initialized = true;
    }
}

[Serializable]
public class BeamAttributes
{
    public AnimationCurve beamDistanceCurve;
    public float beamTravelSpeed = 100f;
    public AnimationCurve beamOpacity;
}