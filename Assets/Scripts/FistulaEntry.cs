using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistulaEntry : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var vessel = collision.GetComponent<VesselController>();
        if (vessel != null)
        {
            vessel.InFistula = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var vessel = collision.GetComponent<VesselController>();
        if (vessel != null)
        {
            vessel.InFistula = vessel.transform.position.y < transform.position.y;
        }

    }
}
