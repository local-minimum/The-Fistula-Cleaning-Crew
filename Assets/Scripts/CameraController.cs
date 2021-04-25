using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    AnimationCurve vesselVerticalTarget;
    [SerializeField] float offsetAmplitude = 3f;
    
    [SerializeField, Range(0, 5)]
    float targetEasing = 2f;

    [SerializeField] float surfaceMinY = 0;
    [SerializeField] float viewDistance = -10f;
    [SerializeField] float maxXDeviation = 4f;
    [SerializeField] float maxY = 2f;
    VesselController vessel;
    Camera cam;
    public CameraShaker shaker { get; private set; }
    
    void Start()
    {
        vessel = FindObjectOfType<VesselController>();
        cam = GetComponentInChildren<Camera>();
        shaker = GetComponentInChildren<CameraShaker>();
    }

    private void LateUpdate()
    {        
        var targetOffset = vesselVerticalTarget.Evaluate(vessel.fallVelocity);

        var targetY = Mathf.Min(vessel.transform.position.y + targetOffset * offsetAmplitude, maxY);
        var targetX = vessel.InFistula ? Mathf.Clamp(vessel.transform.position.x, -maxXDeviation, maxXDeviation) : 0f;
        var pos = new Vector3(
            targetX,
            vessel.InFistula ? targetY : Mathf.Max(targetY, surfaceMinY),
            viewDistance
        );

        transform.position = Vector3.Lerp(transform.position, pos, targetEasing * Time.deltaTime);
    }
}
