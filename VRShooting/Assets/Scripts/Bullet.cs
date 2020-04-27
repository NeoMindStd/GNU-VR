using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 20f;
    [SerializeField] ParticleSystem hitParticlePrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        var velocity = speed * transform.forward;

        var rigidbody = GetComponent<Rigidbody>();

        rigidbody.AddForce(velocity, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other) 
    {
        other.SendMessage("OnHitBullet");

        Instantiate(hitParticlePrefab, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
