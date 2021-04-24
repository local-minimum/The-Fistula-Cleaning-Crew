using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    #region CollisionCounting
    int collisions = 0;
    private float lastCollisionScore = 0f;

    [SerializeField, Range(0, 1)] float collisionGracePeriod = 0.25f;    
    public void GroundCollision()
    {
        if (Time.timeSinceLevelLoad - lastCollisionScore > collisionGracePeriod)
        {
            collisions += 1;
            lastCollisionScore = Time.timeSinceLevelLoad;
        }
    }
    #endregion

    #region CleaningsCounting
    int cleanings = 0;
    public void AddCleaning()
    {
        cleanings += 1;
    }
    #endregion

    #region DurationCounting
    float descentStart = 0f;
    float descentEnd = 0f;
    public void StartDescent()
    {
        descentStart = Time.timeSinceLevelLoad;
    }

    #region landing 
    bool landedStanding = false;
    [SerializeField] float landingAngleTolerange = 10f;
    public void LandingVector(Vector3 vesselUp)
    {
        landedStanding = Mathf.Abs(Vector3.Angle(Vector3.up, vesselUp)) < landingAngleTolerange;
    }

    #endregion
    public void EndDescent()
    {
        descentEnd = Time.timeSinceLevelLoad;
    }
    #endregion


    public ScoreSummary Summarize()
    {
        return new ScoreSummary(collisions, cleanings, descentEnd - descentStart, landedStanding);
    }
}
