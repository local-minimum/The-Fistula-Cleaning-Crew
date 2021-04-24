using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VesselController : MonoBehaviour
{
    [SerializeField] float stabilizingForce = 1f;
    [SerializeField] float stabilizingSpeedScale = 10f;
    [SerializeField] AnimationCurve stabilizingSpeedModulation;

    [SerializeField] float torqueForce = 1f;
    [SerializeField] float liftForce = 1f;

    [SerializeField] float maxTroque = 40f;
    [SerializeField] Vector2 maxVelocity = new Vector2(10, 20);
    
    //TODO: Add easing to clamps
    //[SerializeField, Range(0, 1)] float clampEasing = 0.3f;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        bool stabilize = true;
        if (Input.GetButton("Horizontal"))
        {
            float torque = Mathf.Sign(Input.GetAxis("Horizontal")) * Time.deltaTime * torqueForce;
            rb.AddTorque(torque, ForceMode2D.Force);
            stabilize = false;
        }
        if (Input.GetButton("Vertical"))
        {
            rb.AddForce(transform.up * liftForce * Time.deltaTime, ForceMode2D.Force);
            stabilize = false;
        }
        
        if (stabilize) {
            StabilizeVessel();
        }

        ClampVelocities();
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

    public float fallVelocity
    {
        get
        {
            return rb.velocity.y;
        }
    }

    public bool InFistula
    {
        get;
        set;
    }

    void StabilizeVessel()
    {
        float velocity = rb.angularVelocity;
        float targetVelocity = stabilizingSpeedModulation.Evaluate(Mathf.Abs(rotation)) * Mathf.Sign(rotation) * stabilizingSpeedScale;
        if ((targetVelocity < 0 && velocity > targetVelocity) || (targetVelocity > 0 && velocity < targetVelocity))
        {
            rb.AddTorque(stabilizingForce * Time.deltaTime);
        }        
    }

    void ClampVelocities()
    {
        
        rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -maxTroque, maxTroque);        
        var vel = rb.velocity;
        if (Mathf.Abs(vel.x) > maxVelocity.x)
        {
            vel.x = maxVelocity.x * Mathf.Sign(vel.x);
        }
        if (Mathf.Abs(vel.y) > maxVelocity.y)
        {
            vel.y = maxVelocity.y * Mathf.Sign(vel.y);
        }
        rb.velocity = vel;
    }
}
