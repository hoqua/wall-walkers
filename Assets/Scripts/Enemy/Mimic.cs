using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic : MonoBehaviour
{
    private Vector3Int _spawnPosition;

    private void Start()
    {
        transform.localPosition += new Vector3(0, 0.25f, 0);
    }
    
}
