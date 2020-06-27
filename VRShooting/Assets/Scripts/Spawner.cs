using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnChildDestroyCallback();

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] protected int maxCount = 30;
    
    [SerializeField] protected float spawnCooltme = 1f;

    protected int count = 0;
    
    protected float timer;

    protected void Start()
    {
        timer = Random.Range(0f, spawnCooltme);
    }

    protected void Update()
    {
        timer += Time.deltaTime;
    }

    public void OnChildDestroy()
    {
        count = Mathf.Max(0, count-1);
    }

    protected Vector3 GetSpawonPosition(int min = -50, int max = 50)
    {
        Vector3 position = transform.position;
        position.x += Random.Range(min,max);
        position.z += Random.Range(min,max);
        
        return position;
    }

    public abstract void Spawn();
}
