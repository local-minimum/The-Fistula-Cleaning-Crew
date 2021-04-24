using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ScoreSummary
{
    public int collisions;
    public int cleanings;
    public float flightTime;

    public ScoreSummary(int collisions, int cleanings, float flightTime)
    {
        this.collisions = collisions;
        this.cleanings = cleanings;
        this.flightTime = flightTime;        
    }

    public override string ToString() => string.Format("Score: {0} collisions, {1} cleanings {2} seconds", collisions, cleanings, flightTime);
}
