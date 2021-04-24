using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistulaSegment : MonoBehaviour
{
    [SerializeField] int segmentHeight = 8;

    public Vector3 Bottom {
        get
        {
            return transform.position + Vector3.down * segmentHeight * 0.5f;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.position = Bottom;        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
