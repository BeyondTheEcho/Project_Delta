﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Config
    [Header("Player Variables")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f;
    [SerializeField] int gameOverDelay = 3;

    [Header("Player Weapons")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] AudioClip laserClip;
    [SerializeField] float laserSpeed = 10f;
    [SerializeField] float fireDelay = 0.2f;
    [SerializeField][Range(0, 1)] float laserVol = 0.2f;

    [Header("Player Explosion")]
    [SerializeField] AudioClip explosionClip;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float explosionLifeTime = 1;
    [SerializeField] [Range(0, 1)] float explosionVol = 1f;

    [Header("Shield Variables")]
    [SerializeField] GameObject shieldBar;
    [SerializeField] float shield = 1000f;
    [SerializeField] float shieldBarMax = 1000f;

    [Header("Heat Variables")]
    [SerializeField] GameObject heatBar;
    [SerializeField] float currentHeat = 0.00f;
    [SerializeField] float maxHeat = 1f;
    [SerializeField] float heatIncriment = 0.01f;
    [SerializeField] float heatCooldown = 0.05f;
    [SerializeField] int cooldownPeriod = 1;
    [SerializeField] bool cooldownActive = false;

    // In Script Config / Variables
    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Coroutine References
    Coroutine laserCoroutine;
    Coroutine heatCool;

    // Start is called before the first frame update
    void Start()
    {
        EstablishMoveBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        FireLaser();
    }

    private void FireLaser()
    {
        //When Space is first pressed and held, starts a coroutine
        if (Input.GetButtonDown("Fire1"))
        {
            //If the Reduce Heat coroutine is active, end it
            if (cooldownActive)
            {
                StopCoroutine(heatCool);
            }

            //Starts the FireLaserWhileHeld coroutine
            laserCoroutine = StartCoroutine(FireLaserWhileHeld());
        }
        //When space is released this stops the FireLaserWhileHeld coroutine
        if (Input.GetButtonUp("Fire1"))
        {
            //Stops firing
            StopCoroutine(laserCoroutine);
            //Begins cooling down heat when not firing
            heatCool = StartCoroutine(ReduceHeat());
        }           
    }

    //Coroutine that players while space is held
    IEnumerator FireLaserWhileHeld()
    {
        // While (true) is always true and will run FOREVER
        while (true)
        {
            if (currentHeat <= maxHeat)
            {
                //Creates lasers and imparts velocity
                InstantiateLasers();
                //Plays laser audio clip
                AudioSource.PlayClipAtPoint(laserClip, transform.position, laserVol);
                //Increases Heat With Every Shot
                IncrimentHeat();
            }
            // Yields return for the fireDelay var
            yield return new WaitForSeconds(fireDelay);
        }
    }

    private void InstantiateLasers()
    {
        if (FindObjectOfType<UIManagement>().GetDoubleLasers() == false)
        {
            //Instantiates a laser
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            //Imparts velocity to the new laser
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
        }
        else if (FindObjectOfType<UIManagement>().GetDoubleLasers() == true)
        {
            //Instantiates a laser
            GameObject laser1 = Instantiate(laserPrefab, new Vector2(transform.position.x + 0.5f, transform.position.y), Quaternion.identity) as GameObject;
            //Imparts velocity to the new laser
            laser1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
            //Instantiates a laser
            GameObject laser2 = Instantiate(laserPrefab, new Vector2(transform.position.x - 0.5f, transform.position.y), Quaternion.identity) as GameObject;
            //Imparts velocity to the new laser
            laser2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
        }
    }

    //Incriments Heat
    private void IncrimentHeat()
    {
        if (currentHeat <= maxHeat)
        {
            currentHeat += heatIncriment;
            heatBar.GetComponent<Image>().fillAmount = currentHeat;
        }
    }

    //This coroutine reduces Heat
    IEnumerator ReduceHeat()
    {
        //While True is ALWAYS true
        while (true)
        {
            //Sets the cooldownActive bool to true
            cooldownActive = true;
            //Reduces current by the heatCooldown var
            currentHeat -= heatCooldown;
            //Applies the current heat to the Image fill amount
            heatBar.GetComponent<Image>().fillAmount = currentHeat;
            //Yields return for the cooldownPeriod in Seconds
            yield return new WaitForSeconds(cooldownPeriod);
        }
    }



    private void PlayerMove()
    {
        // Gets Input from Axis in "Unity Project Seetings > Input > Axes"
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        // Calculates the new X and Y Positions by adding the change(Δ : Delta) of X and Y
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        // Acts upon the Player objects transform component
        transform.position = new Vector2(newXPos, newYPos);
    }

    private void EstablishMoveBoundaries()
    {
        // Assigns the variable gameCamera, of type Camera, to the Main Camera object
        Camera gameCamera = Camera.main;

        // Establishes the X min and max values as they relate to the gamespace, relative to the Cameras boundaries
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        // Establishes the Y min and max values as they relate to the gamespace, relative to the Cameras boundaries
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
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
        shield -= damagecontroller.GetDamage();

        // Updates the health bar
        UpdateHealthBar();

        // Destroys the laser on collision
        damagecontroller.Hit();

        //Destroys the object when the health is <= 0
        if (shield <= 0)
        {
            PlayerDie();
        }
    }

    private void PlayerDie()
    {
        //Plays death VFX and SFX
        DeathEffects();
        // Waits for the gameOverDelay and then loads game over
        LoadGameOver();
        //Destroys player
        Destroy(gameObject);
    }

    private void DeathEffects()
    {
        //Plays explosion audio
        AudioSource.PlayClipAtPoint(explosionClip, transform.position, explosionVol);
        //Instatinates Player Explosion VFX
        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation) as GameObject;
        //Destroys explosion after a fixed time - Variable in config
        Destroy(explosion, explosionLifeTime);
    }

    private void LoadGameOver()
    {
        //Starts a coroutine in SceneLoader.cs that waits
        FindObjectOfType<SceneLoader>().LoadGameOver(gameOverDelay);
    }

    //Updates the health bar fill in the Canvas
    private void UpdateHealthBar()
    {
        // Creates a variable that stores the percentage of the health bar fill
        float healthBarFill = shield / shieldBarMax;
        //Sets the fill amount to the percentage of health / total health
        shieldBar.GetComponent<Image>().fillAmount = healthBarFill;
    }
}
