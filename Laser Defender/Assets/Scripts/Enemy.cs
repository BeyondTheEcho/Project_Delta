using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Config
    [SerializeField] float health = 100;
    [SerializeField] float laserTimer;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject enemyLaserPrefab;
    [SerializeField] float enemyLaserVelocity = 6f;

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

        //Manages the damage calculations
        ManageDamage(damageController);
    }

    private void ManageDamage(DamageController damagecontroller)
    {
        // Applies damage to the health of this object
        health -= damagecontroller.GetDamage();

        //Destroys the laser on collision
        damagecontroller.Hit();

        //Destroys the object when the health is <= 0
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
