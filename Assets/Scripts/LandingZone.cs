using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingZone : MonoBehaviour
{
    bool firstEntry = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (firstEntry)
        {
            var vessel = collision.GetComponent<VesselController>();
            if (vessel != null)
            {
                firstEntry = false;
                vessel.LandingSequence();
                HUDLanding.Show();
            }
        }
    }
}
