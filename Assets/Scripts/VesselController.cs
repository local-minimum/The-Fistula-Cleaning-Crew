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
            Debug.Log(string.Format("Rotate {0}", torque));
        }
        if (Input.GetButton("Vertical"))
        {
            rb.AddForce(transform.up * liftForce * Time.deltaTime, ForceMode2D.Force);
            stabilize = false;
            Debug.Log("Fly");
        }
        
        if (stabilize) {
            StabilizeVessel();
        }
        
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
        //Debug.Log(string.Format("{0} {1} {2}", velocity, rotation, targetVelocity));
    }
}
