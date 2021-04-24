using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletingMenuCamera : MonoBehaviour
{
    private void Awake()
    {
        if (FindObjectOfType<CameraController>())
        {
            Destroy(gameObject);
        }
    }
}
