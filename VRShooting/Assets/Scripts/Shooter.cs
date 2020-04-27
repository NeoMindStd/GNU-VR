using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{

    [SerializeField] GameObject bulltetPrefab;
    [SerializeField] Transform gunBarrelEnd;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1")) 
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bulltetPrefab, gunBarrelEnd.position, gunBarrelEnd.rotation);
    }
}
