using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] float magnitude = 0.2f;
    [SerializeField] float gracePeriod = 1f;
    [SerializeField] float shakeDuration = 0.5f;
    float lastShake = 0f;

    public void Shake()
    {
        StartCoroutine(_Shake());
    }

    private IEnumerator<WaitForSeconds> _Shake()
    {
        if (Time.timeSinceLevelLoad - lastShake < gracePeriod)
        {
            yield break;
        }
        lastShake = Time.timeSinceLevelLoad;
        while (Time.timeSinceLevelLoad - lastShake < shakeDuration)
        {
            transform.localPosition = new Vector3(Random.Range(-magnitude, magnitude), Random.Range(-magnitude, magnitude), 0f);
            yield return new WaitForSeconds(0.02f);
        }
        transform.localPosition = Vector3.zero;
    }
}
