using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VesselController : MonoBehaviour
{
    [SerializeField] float stabilizingForce = 1f;
    [SerializeField] float stabilizingSpeedScale = 10f;
    [SerializeField] AnimationCurve stabilizingSpeedModulation;

    [SerializeField] float torqueForce = 1f;
    [SerializeField] float liftForce = 1f;
    [SerializeField] float lateralForce = 0f;

    [SerializeField] float maxTroque = 40f;
    [SerializeField] Vector2 maxVelocity = new Vector2(10, 20);

    [SerializeField] ParticleSystem mainRocket;
    [SerializeField] ParticleSystem leftRocket;
    [SerializeField] ParticleSystem rightRocket;
    [SerializeField] bool enableStabilize = false;

    CameraController cam;

    bool playerHasControl = true;
    public bool PlayerPlaying
    {
        get
        {
            return playerHasControl;
        }
    }

    public ScoreKeeper scoreKeeper {
        private set;
        get;
    }
 

    //TODO: Add easing to clamps
    //[SerializeField, Range(0, 1)] float clampEasing = 0.3f;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<CameraController>();
        rb = GetComponentInChildren<Rigidbody2D>();
        scoreKeeper = GetComponentInChildren<ScoreKeeper>();
        if (scoreKeeper == null)
        {
            scoreKeeper = gameObject.AddComponent<ScoreKeeper>();
        }
        mainRocket.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        leftRocket.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        rightRocket.Stop(false, ParticleSystemStopBehavior.StopEmitting);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }
        bool stabilize = true;
        if (playerHasControl && Input.GetButton("Horizontal"))
        {
            float direction = Mathf.Sign(Input.GetAxis("Horizontal"));
            float torque = direction * Time.deltaTime * torqueForce;
            float lateralForce = direction * Time.deltaTime * this.lateralForce;
            rb.AddTorque(torque, ForceMode2D.Force);
            rb.AddForce(Vector2.right * lateralForce);
            if (direction > 0f)
            {
                if (leftRocket.isStopped) leftRocket.Play();
                if (rightRocket.isPlaying) rightRocket.Stop(false, ParticleSystemStopBehavior.StopEmitting);

            } else
            {
                if (leftRocket.isPlaying) leftRocket.Stop(false, ParticleSystemStopBehavior.StopEmitting);
                if (rightRocket.isStopped) rightRocket.Play();
            }
            stabilize = false;
        } else
        {
            leftRocket.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            rightRocket.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        }

        if (playerHasControl && Input.GetButton("Vertical"))
        {
            rb.AddForce(transform.up * liftForce * Time.deltaTime, ForceMode2D.Force);
            stabilize = false;
            mainRocket.Play();
        } else
        {
            mainRocket.Stop(false, ParticleSystemStopBehavior.StopEmitting);
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
        if (!enableStabilize) return;
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

    [SerializeField] float maxLandingSpeed = 1f;
    [SerializeField] float maxLandingTorque = 5f;

    bool landingVelocity
    {
        get
        {
            return rb.velocity.magnitude < maxLandingSpeed && Mathf.Abs(rb.angularVelocity) < maxLandingTorque;
        }
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            SoundBoard.Play(SoundType.BouncingGround);
            scoreKeeper.GroundCollision();
            cam.shaker.Shake();
        } else if (collision.gameObject.tag == "Bottom" || collision.gameObject.tag == "Surface")
        {
            SoundBoard.Play(SoundType.BouncingGround);
            cam.shaker.Shake();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (playerHasControl && collision.gameObject.tag == "Bottom" && landingVelocity)
        {
            scoreKeeper.EndDescent();
            scoreKeeper.LandingVector(transform.up);
            playerHasControl = false;
            var score = scoreKeeper.Summarize();
            Debug.Log(score.ToString());
            SceneManager.LoadScene("WorkReport", LoadSceneMode.Additive);
        }
    }
}
