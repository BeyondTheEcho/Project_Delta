using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torpedo : MonoBehaviour
{
    //Config
    [SerializeField] int torpedoDelay = 3;
    [SerializeField] int detonationRadius = 3;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] AudioClip explosionClip;
    [SerializeField] [Range(0, 1)] float explosionVol = 1;

    // Start is called before the first frame update
    void Start()
    {
        //Starts the delay (fuse) when instantiated
        StartCoroutine(Detonate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    IEnumerator Detonate()
    {
        // Waits for the delay
        yield return new WaitForSeconds(torpedoDelay);
        //Deals splash damage
        ExplosionDamage();
        //Triggers the death effects
        TriggerDeathEffects();
        //destroys this game object
        Destroy(gameObject);
    }

    private void TriggerDeathEffects()
    {
        //Instatinates Explosion VFX
        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation) as GameObject;
        //Destroys explosion after a fixed time - Variable in config
        Destroy(explosion, 1);
        //Plays explosion audio
        AudioSource.PlayClipAtPoint(explosionClip, transform.position, explosionVol);
    }

    private void ExplosionDamage()
    {
        //Creates an array of type collider2D and assigns it the colliders of all objects overlapped by the circle
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detonationRadius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            //Checks to see if the colliders objects have the Enemy tag
            if (hitColliders[i].tag == "Enemy")
            {
                //Triggers ManageDamage() in Enemy.cs and passes in the required DamageController.cs connected to this object
                hitColliders[i].SendMessage("ManageDamage", gameObject.GetComponent<DamageController>());            
            }
            i++;
        }
    }
}
