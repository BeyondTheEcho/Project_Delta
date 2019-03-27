using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    //Config
    [SerializeField] int damage = 100;
    
    public int GetDamage()
    {
        return damage;
    }

    public void Hit()
    {
        if (gameObject.GetComponent<Enemy>())
        {
            gameObject.GetComponent<Enemy>().TriggerDeath();
        }
        Destroy(gameObject);
    }
}
