using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuilderInstruction
{
    public FistulaSegment segment;
    public bool flipped;

    public BuilderInstruction(FistulaSegment seg)
    {
        segment = seg;
        flipped = false;
    }

    public BuilderInstruction(FistulaSegment seg, bool flip)
    {
        segment = seg;
        flipped = flip;
    }

    public bool equals(BuilderInstruction other) => other.segment.SegmentID == segment.SegmentID && other.flipped == flipped;

    public string toString()
    {
        return string.Format("{0}/{1} {2}", segment.SegmentID, segment.name, flipped);
    }
}
