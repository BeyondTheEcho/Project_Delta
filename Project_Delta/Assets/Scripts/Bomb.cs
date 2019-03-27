using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] AudioClip explosionClip;
    [SerializeField] [Range(0, 1)] float explosionVol;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            TriggerBombEffects();
        }
    }

    private void TriggerBombEffects()
    {
        //Plays explosion audio
        AudioSource.PlayClipAtPoint(explosionClip, transform.position, explosionVol);
        //Creates an instance of the bomb explosion
        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation) as GameObject;
        //Destroys explosion after 1 second
        Destroy(explosion, 1);
    }
}
