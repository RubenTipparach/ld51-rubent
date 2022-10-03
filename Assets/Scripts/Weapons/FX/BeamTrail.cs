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
    Vector3 originPoint = Vector3.zero;
    Vector3 destPoint = Vector3.zero;

    public float angleOfFire = 30f;

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


            if (destination == null)
            {
                distanceRatio = 1;
            }

            if(origin== null)
            {
                Destroy(gameObject);
                return;
            }

            if(noHit)
            {
                destPoint = origin.forward * maxRange + origin.position;
            }

            line.SetPosition(0, originPoint);

            var headingAndDistance = (destPoint - originPoint);
            var direction = headingAndDistance.normalized 
                * Mathf.Clamp(headingAndDistance.magnitude,
                0,
                maxRange)
                * distanceRatio;

            line.SetPosition(1, originPoint + direction);
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
                    Debug.Log("Beam fire struck hull!");
                    weaponContactCallback.Invoke(); // determine accuracy if needed.
                }
            }

            if (destination != null && origin != null)
            {
                originPoint = origin.position;
                destPoint = destination.position;
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
        Transform destination, float range, float angle = 30)
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
            //weaponContactCallback = null;
        }

        if (destination != null && origin != null)
        {
            originPoint = origin.position;
            destPoint = destination.position;
        }

        var beamRotation = Quaternion.LookRotation((destPoint - originPoint).normalized);

        if(Quaternion.Angle(beamRotation, origin.rotation) > angleOfFire)
        {
            noHit = true;
        }

        beamDistanceTimer.duration = beamDistance / beamAttributes.beamTravelSpeed + afterGlowTime;

        beamDistanceTimer.Init();
        //Debug.Log("time for beam " + beamDistanceTimer.duration + " beam dist " + beamDistance);

        transform.position = origin.position;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);

        initialized = true;
        maxRange = range;
        angleOfFire = angle;
    }
}

[Serializable]
public class BeamAttributes
{
    public AnimationCurve beamDistanceCurve;
    public float beamTravelSpeed = 100f;
    public AnimationCurve beamOpacity;
}