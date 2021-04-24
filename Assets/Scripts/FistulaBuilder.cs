using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistulaBuilder : MonoBehaviour
{
    [SerializeField] FistulaSegment firstSegment;

    void Start()
    {
        Instantiate<FistulaSegment>(firstSegment, transform.position, Quaternion.identity, transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
