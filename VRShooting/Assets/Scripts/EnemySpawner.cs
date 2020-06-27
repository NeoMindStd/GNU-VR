using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Spawner
{
    [SerializeField] Enemy[] enemyPrefabs;
    [SerializeField] Enemy.Type[] enemyTypes;

    public void OnChildDestroy()
    {
        count = Mathf.Max(0, count-1);
    }

    public override void Spawn()
    {
        Vector3 position = base.GetSpawonPosition();
        int index = Random.Range(0,enemyPrefabs.Length);
        if(count < maxCount && timer >= spawnCooltme) 
        {
            timer = 0;
            Enemy enemy = Instantiate(enemyPrefabs[index], position, transform.rotation);
            enemy.type = enemyTypes[index];
            enemy.SetSpawnerDelegate(new OnChildDestroyCallback(OnChildDestroy));
            count++;
        }
    }
}
