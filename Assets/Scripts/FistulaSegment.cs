using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistulaSegment : MonoBehaviour
{
    [SerializeField] int segmentHeight = 8;
    [SerializeField] string upEdge;
    [SerializeField] string downEdge;
    [SerializeField] string segmentId;

    public string SegmentID
    {
        get
        {
            return segmentId;
        }
    }

    public string Up
    {
        get
        {
            return upEdge;
        }
    }

    public string Down
    {
        get
        {
            return downEdge;
        }
    }

    public Vector3 Bottom {
        get
        {
            return transform.position + Vector3.down * segmentHeight * 0.5f;
        }
    }
}
