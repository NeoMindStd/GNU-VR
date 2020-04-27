using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] Enemy enemyPrefab;

    Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        if(enemy==null)
         {
             enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
         }
    }
}
