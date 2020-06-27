using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSpawner : Spawner
{
    [SerializeField] Potion[] potionPrefabs;

    public void OnChildDestroy()
    {
        count = Mathf.Max(0, count-1);
    }

    public override void Spawn()
    {
        int index = Random.Range(0,potionPrefabs.Length);
        if(count < maxCount && timer >= spawnCooltme) 
        {
            timer = 0;
            Vector3 position = GetSpawonPosition(0, 0);
            position.y = potionPrefabs[index].transform.position.y;
            Potion potion = Instantiate(potionPrefabs[index], position, potionPrefabs[index].transform.rotation);
            potion.SetSpawnerDelegate(new OnChildDestroyCallback(OnChildDestroy));
            count++;
        }
    }
}
