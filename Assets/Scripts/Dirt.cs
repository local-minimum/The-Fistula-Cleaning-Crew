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
        if (!cleaning)
        {
            SoundBoard.Play(SoundType.TargetGunk);
            StartCoroutine(PlayCleaning());
        }
        cleaningTarget = this;
    }

    IEnumerator<WaitForSeconds> PlayCleaning()
    {
        yield return new WaitForSeconds(0.4f);
        Laser.Target(transform);
        yield return new WaitForSeconds(0.1f);
        SoundBoard.Play(SoundType.CleaningGunk);
        
    }

    private void Update()
    {
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
                Cleaned();
                SoundBoard.Play(SoundType.ExplodingGunk);
            }
        } else
        {
            cleanness = Mathf.Max(cleanness - Time.deltaTime * decay, 0f);
        }
    }

    private void OnBecameInvisible()
    {
        if (cleaning)
        {
            cleaningTarget = null;
            Laser.ClearTarget();
        }
    }

    private void Cleaned()
    {
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
