using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ScoreSummary
{
    public int collisions;
    public int cleanings;
    public float flightTime;
    public bool standing;

    public ScoreSummary(int collisions, int cleanings, float flightTime, bool standing)
    {
        this.collisions = collisions;
        this.cleanings = cleanings;
        this.flightTime = flightTime;
        this.standing = standing;
    }

    public override string ToString() => string.Format("Score: {0} collisions, {1} cleanings {2} seconds, {3}", collisions, cleanings, flightTime, standing ? "standing" : "not standing");
}
