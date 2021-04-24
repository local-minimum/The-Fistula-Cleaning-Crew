using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistulaSegment : MonoBehaviour
{
    [SerializeField] int segmentHeight = 8;
    [SerializeField] string upEdge;
    [SerializeField] string downEdge;
    [SerializeField] string segmentId;
    [SerializeField] List<Transform> stationaryDirtSpawns = new List<Transform>();

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

    public int SpawnDirt(int count)
    {
        List<int> free = new List<int>();
        for (int i=0; i<stationaryDirtSpawns.Count; i++)
        {
            free.Add(i);
        }
        int j = 0;
        while (j < count && free.Count > 0)
        {
            var idFree = Random.Range(0, free.Count);
            var idPos = free[idFree];
            free.RemoveAt(idFree);
            _spawnDirt(stationaryDirtSpawns[idPos]);
            j++;
        }
        return j;
    }

    private void _spawnDirt(Transform parent)
    {
        var prefab = DirtRepocitory.GetStationaryPrefab();
        var child = Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
        child.transform.position = parent.transform.position;
    }
}
