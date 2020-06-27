using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour 
{
    [SerializeField] AudioClip getItemSound;

    public string itemName;
    protected AudioSource audioSource;
    protected OnChildDestroyCallback spawnerDelegate;

    protected void Start() 
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(RotateItemCoroutine());
    }

    protected void OnDestroy() 
    {
        spawnerDelegate();
    }

    public void SetSpawnerDelegate(OnChildDestroyCallback spawnerDelegate)
    {
        this.spawnerDelegate = spawnerDelegate;
    }

    IEnumerator RotateItemCoroutine()
    {
        while(true)
        {
            gameObject.transform.Rotate(0f, 4f, 0f, Space.World);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void OnGetItem(PlayerController playerController)
    {
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<Renderer>().enabled = false;

        if(audioSource != null && getItemSound != null) 
        {
            audioSource.PlayOneShot(getItemSound);
            Debug.Log("Sound Played");
        }

        Debug.Log("on get item");

        Destroy(gameObject, 2f);
    }
}
