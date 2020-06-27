using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{    
    public enum Type
    {
        zombear,
        zombunny
    }

    [SerializeField] AudioClip spawnClip;
    [SerializeField] AudioClip hitClip;

    [SerializeField] Collider enemyCollider;
    [SerializeField] Renderer enemyRenderer;
    [SerializeField] ParticleSystem hitParticlePrefab;

    AudioSource audioSource;

    Animator anim;

    private NavMeshAgent navAgent;

    private bool isDead = false;

    OnChildDestroyCallback spawnerDelegate;

    public Type type;

    // Start is called before the first frame update
    void Start()
    {
        enemyCollider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();

        if(spawnClip!=null) audioSource.PlayOneShot(spawnClip);
    }

    // Update is called once per frame
    void Update()
    {
        Chase();
    }

    public void SetSpawnerDelegate(OnChildDestroyCallback spawnerDelegate)
    {
        this.spawnerDelegate = spawnerDelegate;
    }

    private void Chase()
    {
        if(!isDead) navAgent.destination = PlayerController.player.transform.position;
    }

    void OnHit(Transform hitTransform)
    {
        Instantiate(hitParticlePrefab, hitTransform.position, hitTransform.rotation);

        if(!isDead) 
        {
            isDead = true;
            enemyCollider.enabled = false;

            if(type == Type.zombear)
            {
                DeadScreenController.killZombearCount++;
            }
            else if(type == Type.zombunny)
            {
                DeadScreenController.killZombunnyCount++;
            }

            StartCoroutine(DestroyCoroutine());
            spawnerDelegate();

            navAgent.isStopped = true;

            anim.SetTrigger("Dead");

            if(hitClip!=null) audioSource.PlayOneShot(hitClip);
        }
    }

    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(2);
        enemyRenderer.enabled = false;
        Destroy(gameObject, 1f);
    }
}
