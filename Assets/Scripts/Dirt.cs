using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dirt : MonoBehaviour
{
    [SerializeField] bool mobile;
    [SerializeField] float cleaningDuration = 1f;
    [SerializeField, Range(0, 1)] float decay = 0.2f;

    static Dirt cleaningTarget;

    float cleanness = 0f;    
    bool cleaned = false;
    VesselController vessel;

    public bool Mobile {
        get { return mobile; }
    }

    public bool cleaning
    {
        get
        {
            return cleaningTarget == this;
        }
    }

    private void Start()
    {
        vessel = FindObjectOfType<VesselController>();
    }

    [SerializeField] float noCleanStartDuration = 0.5f;

    private void OnMouseEnter()
    {
        if (Time.timeSinceLevelLoad < noCleanStartDuration) return;
        if (!cleaning && vessel.PlayerPlaying)
        {
            isLasered = false;
            SoundBoard.Play(SoundType.TargetGunk);
            StartCoroutine(PlayCleaning());
        }
        cleaningTarget = this;
    }

    IEnumerator<WaitForSeconds> PlayCleaning()
    {
        yield return new WaitForSeconds(0.4f);
        Laser.Target(transform);
        isLasered = true;
        yield return new WaitForSeconds(0.1f);
        SoundBoard.Play(SoundType.CleaningGunk);
        
    }

    bool isLasered = false;

    private void Update()
    {
        if (!vessel.PlayerPlaying)
        {
            Laser.ClearTarget();
            return;
        }

        if (cleaned) return;
        if (cleaning)
        {
            if (!vessel.PlayerPlaying)
            {
                Laser.ClearTarget();
                return;
            }
            cleanness += Time.deltaTime;
            if (cleanness > cleaningDuration)
            {
                cleaned = true;
                StartCoroutine(Cleaned());
            } else if (isLasered)
            {
                Vibrate();
            }
        } else
        {
            cleanness = Mathf.Max(cleanness - Time.deltaTime * decay, 0f);
        }
    }

    float vibrationMagnitude = 0.2f;
    void Vibrate()
    {
        transform.localScale = new Vector3(Random.Range(1 - vibrationMagnitude, 1 + vibrationMagnitude), Random.Range(1 - vibrationMagnitude, 1 + vibrationMagnitude), 1f);
    }

    private void OnBecameInvisible()
    {
        if (cleaning)
        {
            cleaningTarget = null;
        }
    }

    private IEnumerator<WaitForSeconds> Cleaned()
    {
        SoundBoard.Play(SoundType.ExplodingGunk);
        float start = Time.timeSinceLevelLoad;
        while (Time.timeSinceLevelLoad - start < 0.5f)
        {
            Vibrate();
            yield return new WaitForSeconds(0.02f);
        }
        Laser.ClearTarget();        
        vessel.scoreKeeper.AddCleaning();        
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (cleaning)
        {
            cleaningTarget = null;
        }
    }
}
