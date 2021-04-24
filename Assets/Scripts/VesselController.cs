using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VesselController : MonoBehaviour
{
    [SerializeField] float stabilizingForce = 1f;
    [SerializeField] float stabilizingSpeedScale = 10f;
    [SerializeField] AnimationCurve stabilizingSpeedModulation;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        StabilizeVessel();    
    }

    float rotation
    {
        get
        {
            float rotation = Vector3.Angle(transform.up, Vector3.up);            
            if (rotation < -180f) {
                return rotation + 180f;
            } else if (rotation > 180f)
            {
                return rotation - 180f;
            }
            return rotation;
        }
    }

    void StabilizeVessel()
    {
        float velocity = rb.angularVelocity;
        float targetVelocity = stabilizingSpeedModulation.Evaluate(Mathf.Abs(rotation)) * Mathf.Sign(rotation) * stabilizingSpeedScale;
        if ((targetVelocity < 0 && velocity > targetVelocity) || (targetVelocity > 0 && velocity < targetVelocity))
        {
            rb.AddTorque(stabilizingForce * Time.deltaTime);
        }
        Debug.Log(string.Format("{0} {1} {2}", velocity, rotation, targetVelocity));
    }
}
