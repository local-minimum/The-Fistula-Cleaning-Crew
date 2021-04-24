using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    static Laser instance { get; set; }
   
    public static void ClearTarget()
    {
        instance.target = null;
    }

    public static void Target(Transform target)
    {
        instance.target = target;
    }

    LineRenderer lr;
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(this);
        }
    }

    void Start()
    {
        lr = GetComponentInChildren<LineRenderer>();
    }

    Transform target;
    bool targeting
    {
        get
        {
            return target != null;
        }
    }

    Vector3 targetDirt
    {
        get
        {
            return transform.InverseTransformPoint(target.position);
        }
    }

    void Update()
    {
        if (targeting) {
            lr.enabled = true;
            lr.SetPosition(1, targetDirt);            
        } else
        {
            lr.enabled = false;
        }        
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
