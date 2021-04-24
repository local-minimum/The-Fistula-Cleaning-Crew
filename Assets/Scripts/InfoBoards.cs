using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoBoards : MonoBehaviour
{
    [SerializeField] List<GameObject> boards = new List<GameObject>();
    int index = -1;

    private void Start()
    {
        OnClickNext();
    }

    public void OnClickNext()
    {
        if (index >= 0)
        {
            boards[index].SetActive(false);
        }
        index += 1;
        if (index >= boards.Count)
        {
            index = 0;
        }
        boards[index].SetActive(true);
    }
}
