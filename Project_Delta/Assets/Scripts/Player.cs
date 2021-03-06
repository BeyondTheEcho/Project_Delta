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
    [SerializeField] GameObject torpedoCount;
    [SerializeField] AudioClip laserClip;
    [SerializeField] float laserSpeed = 10f;
    [SerializeField] int remainingTorpedoes = 3;
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
    [SerializeField] GameObject heatText;
    [SerializeField] float currentHeat;
    [SerializeField] float maxHeat = 1f;
    [SerializeField] float heatIncriment = 0.01f;
    [SerializeField] float heatCooldown = 0.05f;
    [SerializeField] int cooldownInterval = 1;
    [SerializeField] int cooldownPeriod = 5;
    [SerializeField] bool cooldownActive;
    [SerializeField] bool isOverHeated;

    [Header("Shield Variables")]
    [SerializeField] GameObject torpedoPrefab;
    [SerializeField] int torpedoSpeed = 5;

    // In Script Config / Variables
    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Coroutine References
    Coroutine laserCoroutine;
    Coroutine overHeat;
    Coroutine coolDown;

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
        OverHeat();
        LaunchTorpedo();
        UpdateTorpedoCount();
    }

    //This coroutine reduces Heat
    IEnumerator ReduceHeat()
    {
        while (true)
        {
            //Sets the cooldownActive bool to true
            cooldownActive = true;
            //Yields return for the cooldownPeriod in Seconds
            yield return new WaitForSeconds(cooldownInterval);
            //applies cool down
            currentHeat -= heatCooldown;
            //clamps currentHeat between 0 and 1
            currentHeat = Mathf.Clamp(currentHeat, 0, 1);
            //Applies the current heat to the Image fill amount
            heatBar.GetComponent<Image>().fillAmount = currentHeat;
            //Sets the cooldownActive bool to false
            cooldownActive = false;
        }
    }

    private void OverHeat()
    {
        if(currentHeat >= maxHeat)
        {
            overHeat = StartCoroutine(OverHeatPeriod());
        }
    }

    IEnumerator OverHeatPeriod()
    {
        isOverHeated = true;
        heatText.SetActive(true);
        yield return new WaitForSeconds(cooldownPeriod);
        isOverHeated = false;
        heatText.SetActive(false);
        StopCoroutine(overHeat);
    }

    private void FireLaser()
    {
        //When Space is first pressed and held, starts a coroutine
        if (Input.GetButtonDown("Fire1"))
        {
            //Starts the FireLaserWhileHeld coroutine
            laserCoroutine = StartCoroutine(FireLaserWhileHeld());
            // Stops the cooldown if it is active
            if(cooldownActive == true) StopCoroutine(coolDown);
        }
        //When space is released this stops the FireLaserWhileHeld coroutine
        if (Input.GetButtonUp("Fire1"))
        {
            //Stops firing
            StopCoroutine(laserCoroutine);
            // Starts the cooldown cycle
            coolDown = StartCoroutine(ReduceHeat());
        }           
    }

    //Coroutine that players while space is held
    IEnumerator FireLaserWhileHeld()
    {
        // While (true) is always true and will run FOREVER
        while (true)
        {
            if (isOverHeated == false)
            {          
                //Creates lasers and imparts velocity
                InstantiateLasers();
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

        //Plays laser audio clip
        AudioSource.PlayClipAtPoint(laserClip, transform.position, laserVol);
    }

    //Incriments Heat
    private void IncrimentHeat()
    {
        if (currentHeat <= maxHeat)
        {
            //Increases currentHeat
            currentHeat += heatIncriment;
            //Clamps currentHeat between 0 and 1
            currentHeat = Mathf.Clamp(currentHeat, 0, 1);
            //Applies the new heat
            heatBar.GetComponent<Image>().fillAmount = currentHeat;
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

    public void SetHeatCooldown(float heatCool)
    {
        heatCooldown = heatCool;
    }

    private void LaunchTorpedo()
    {
        //When Left Alt is first pressed 
        if (Input.GetButtonDown("Fire2"))
        {
            //Check to see if the player has torpedoes
            if (remainingTorpedoes > 0)
            {
                //Instantiates a Torpedo
                GameObject torpedo = Instantiate(torpedoPrefab, transform.position, Quaternion.identity) as GameObject;
                //Imparts velocity to the new laser
                torpedo.GetComponent<Rigidbody2D>().velocity = new Vector2(0, torpedoSpeed);
                //Subtracts 1 torpedo
                remainingTorpedoes -= 1;
            }
        }

    }

    private void UpdateTorpedoCount()
    {
        torpedoCount.GetComponent<TMPro.TMP_Text>().text = remainingTorpedoes.ToString();
    }
}
