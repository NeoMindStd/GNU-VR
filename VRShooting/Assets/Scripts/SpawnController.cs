using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    Spawner[] spawners;

    // Start is called before the first frame update
    void Start()
    {
        spawners = GetComponentsInChildren<Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < spawners.Length; i++)
        {
            spawners[i].Spawn();
        }
    }
}
