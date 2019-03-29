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
        StartCoroutine(Detonate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Detonate()
    {
        yield return new WaitForSeconds(torpedoDelay);
        ExplosionDamage();
        TriggerDeathEffects();
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
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detonationRadius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Enemy")
            {
                hitColliders[i].SendMessage("ManageDamage", gameObject.GetComponent<DamageController>());            
            }
            i++;
        }
    }
}
