using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMGSpawner : Spawner
{
    [SerializeField] SMG[] smgPrefabs;

    public void OnChildDestroy()
    {
        count = Mathf.Max(0, count-1);
    }

    public override void Spawn()
    {
        int index = Random.Range(0,smgPrefabs.Length);
        if(count < maxCount && timer >= spawnCooltme) 
        {
            timer = 0;
            Vector3 position = GetSpawonPosition(0, 0);
            position.y = smgPrefabs[index].transform.position.y;
            SMG smg = Instantiate(smgPrefabs[index], position, smgPrefabs[index].transform.rotation);
            smg.SetSpawnerDelegate(new OnChildDestroyCallback(OnChildDestroy));
            count++;
        }
    }
}
