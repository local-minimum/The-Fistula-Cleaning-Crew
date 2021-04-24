using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    AnimationCurve vesselVerticalTarget;
    [SerializeField] float offsetAmplitude = 3f;

    [SerializeField, Range(0, 1)]
    float targetEasing = 0.5f;

    [SerializeField] float surfaceMinY = 0;
    [SerializeField] float viewDistance = -10f;

    VesselController vessel;
    Camera cam;
    
    void Start()
    {
        vessel = FindObjectOfType<VesselController>();
        cam = GetComponentInChildren<Camera>();
    }

    private void LateUpdate()
    {        
        var targetOffset = vesselVerticalTarget.Evaluate(vessel.fallVelocity);

        var targetY = vessel.transform.position.y + targetOffset * offsetAmplitude;

        var pos = new Vector3(
            0,
            vessel.InFistula ? targetY : Mathf.Max(targetY, surfaceMinY),
            viewDistance
        );

        transform.position = Vector3.Lerp(transform.position, pos, targetEasing);
    }
}
