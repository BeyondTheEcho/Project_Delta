using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Config
    [SerializeField] float health = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Is triggered on collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Assigns the damagecontroller var to the DamageController.cs attached to the object that collides
        DamageController damagecontroller = collision.gameObject.GetComponent<DamageController>();

        //Manages the damage calculations
        ManageDamage(damagecontroller);
    }

    private void ManageDamage(DamageController damagecontroller)
    {
        // Applies damage to the health of this object
        health -= damagecontroller.GetDamage();

        //Destroys the object when the health is <= 0
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
