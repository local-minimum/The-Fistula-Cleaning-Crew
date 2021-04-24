using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtRepocitory : MonoBehaviour
{
    [SerializeField] List<Dirt> stationary = new List<Dirt>();
    [SerializeField] List<Dirt> mobile = new List<Dirt>();

    static DirtRepocitory _instance;
    static DirtRepocitory instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            _instance = this;
        } else if (instance != this)
        {
            Debug.LogError(string.Format("{0} had duplicate dirt repo", name));
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    public static Dirt GetStationaryPrefab()
    {
        return instance.stationary[Random.Range(0, instance.stationary.Count)];
    }

    public static Dirt GetMobilePrefab()
    {
        return instance.stationary[Random.Range(0, instance.mobile.Count)];
    }

}
