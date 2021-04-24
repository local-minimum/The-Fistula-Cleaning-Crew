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

    private void OnMouseEnter()
    {
        cleaningTarget = this;
        Debug.Log(string.Format("cleaning: {0}", name));
    }

    private void Update()
    {
        if (cleaned) return;
        if (cleaning)
        {
            cleanness += Time.deltaTime;
            if (cleanness > cleaningDuration)
            {
                cleaned = true;
                Cleaned();
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
        }
    }

    private void Cleaned()
    {
        //TODO: Fancy splosh
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
