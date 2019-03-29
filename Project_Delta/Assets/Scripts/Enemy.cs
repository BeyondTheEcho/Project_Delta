using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Config
    [Header("Enemy Config")]
    [SerializeField] float health = 100;
    [SerializeField] int scoreValue = 10;

    [Header("Laser Config")]
    [SerializeField] GameObject enemyLaserPrefab;
    [SerializeField] AudioClip laserClip;
    [SerializeField] float laserTimer;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] float enemyLaserVelocity = 6f;
    [SerializeField][Range(0, 1)] float laserVol = 1f;

    [Header("Explosion Config")]
    [SerializeField] AudioClip explosionClip;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float explosionLifeTime = 1f;
    [SerializeField][Range(0, 1)] float explosionVol = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        //Initializes the laser timer at a random value between the min/max
        laserTimer = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        //Begins the count down before firing
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        // Makes the timer framerate independent
        laserTimer -= Time.deltaTime;
        //Fires the laser when the timer hits 0 OR dips below zero
        if (laserTimer <= 0f)
        {
            FireZeMissiles();
            //Resets the laserTimer for a second shot
            laserTimer = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void FireZeMissiles()
    {
        // Instantiates an enemy laser prefab
        GameObject laser = Instantiate(enemyLaserPrefab, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -enemyLaserVelocity);

        //Plays laser audio
        AudioSource.PlayClipAtPoint(laserClip, transform.position, laserVol);
    }

    // Is triggered on collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Assigns the damagecontroller var to the DamageController.cs attached to the object that collides
        DamageController damageController = collision.gameObject.GetComponent<DamageController>();

        // Added to prevent null reference in certain situations
        if (!damageController)
        {
            return;
        }

        //Checks if the object that collided is a torpedo
        if (collision.tag == "Torpedo")
        {
            //If so this triggers the splash damage and death effects of the torpedo
            collision.SendMessage("TriggerDeathEffects");
            collision.SendMessage("ExplosionDamage");
        }

        //Manages the damage calculations
        ManageDamage(damageController);
    }

    public void ManageDamage(DamageController damagecontroller)
    {
        // Applies damage to the health of this object
        health -= damagecontroller.GetDamage();

        //Destroys the laser on collision
        damagecontroller.Hit();

        //Destroys the object when the health is <= 0
        if (health <= 0)
        {
            //Triggers death function
            TriggerDeath();
        }
    }

    public void TriggerDeath()
    {
        //Triggers Death VFX/SFX
        TriggerDeathEffects();
        //Updates Score
        IncreaseScore();
        //Destroys this game object
        Destroy(gameObject);
    }

    private void TriggerDeathEffects()
    {
        //Instatinates Explosion VFX
        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation) as GameObject;
        //Destroys explosion after a fixed time - Variable in config
        Destroy(explosion, explosionLifeTime);
        //Plays explosion audio
        AudioSource.PlayClipAtPoint(explosionClip, transform.position, explosionVol);
    }

    private void IncreaseScore()
    {
        //GameSession is a singleton and should always exist.
        //Calls the UpdateScore Method and passes in the value of this enemy to be added to the score
        FindObjectOfType<GameSession>().UpdateScore(scoreValue);
    }
}
