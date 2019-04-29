using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarpManager : MonoBehaviour
{
    [Header("Warp Variables")]
    [SerializeField] float currentWarpCharge = 0;
    [SerializeField] float warpFill = 0f;

    //Coroutines
    Coroutine warpTimer;

    // Start is called before the first frame update
    void Start()
    {
        warpTimer = StartCoroutine(WarpTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WarpTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            //Adds 1 to the current charge per cycle
            currentWarpCharge++;

            //The warpTimer variable is serialized on the player and is measured in seconds
            warpFill = currentWarpCharge / FindObjectOfType<Player>().warpTimer;

            //Clamps fill between the min and max 
            Mathf.Clamp(warpFill, 0, 1);
            gameObject.GetComponent<Image>().fillAmount = warpFill;

            //Kills coroutine and warps when the bar is filled
            if (warpFill == 1.0f)
            {
                StopCoroutine(warpTimer);
                Warp();
            }
        }
    }

    private void Warp()
    {
        FindObjectOfType<GameSession>().SaveUpgrades();
        FindObjectOfType<SceneLoader>().LoadPassedScene("Map Screen");
    }
}
