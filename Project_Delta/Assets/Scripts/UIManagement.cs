using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManagement : MonoBehaviour
{
    [Header("Game Status")]
    [SerializeField] bool isPaused = false;
    [SerializeField] GameObject upgradeCanvas;
    [SerializeField] string purchasedText = "Purchased";

    [Header("Double Lasers")]
    [SerializeField] GameObject doubleLasersText;
    [SerializeField] bool doubleLasers = false;
    [SerializeField] int doubleLasersCost = 1;

    [Header("Heat Venting")]
    [SerializeField] GameObject heatVentingText;
    [SerializeField] int heatVentingCost = 1;
    [SerializeField] int heatVentingIncriment = 1;
    [SerializeField] int currentVentingTier = 0;
    [SerializeField] float heatVentingRate = 0.05f;
    [SerializeField] float heatVentingRateIncriment = 0.05f;

    //Stored References
    GameSession gameSession;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectReferences();
    }

    private void FindObjectReferences()
    {
        gameSession = FindObjectOfType<GameSession>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause()
    {
        isPaused = !isPaused;

        if (isPaused == true)
        {        
            //Sets timescale to effectively zero
            Time.timeScale = .0000001f;
            //Enables the upgrade canvas
            upgradeCanvas.SetActive(true);
        }
        else if (isPaused == false)
        {
            //Reverts to normal time
            Time.timeScale = 1.0f;
            //Hides upgrade canvas
            upgradeCanvas.SetActive(false);
        }
    }

    public void DoubleLasers()
    {
        //If doublelasers have not been purchased
        if (doubleLasers == false)
        {
            //If player can afford doublelasers
            if(gameSession.GetScore() >= doubleLasersCost)
            {
                //Sets purchased status to true
                doubleLasers = true;
                //Updates Text / Color
                UpdatedPurchaseText(doubleLasersText);
                //Subtracts Score
                gameSession.UpdateScore(-doubleLasersCost);
            }
            else
            {
                //Play Error Sound
            }
        }
    }

    private void UpdatedPurchaseText(GameObject text)
    {
        text.GetComponent<TMP_Text>().color = Color.green;
        text.GetComponent<TMP_Text>().text = purchasedText;
    }

    public bool GetDoubleLasers()
    {
        return doubleLasers;
    }

    public void HeatVenting()
    {
        if (gameSession.GetScore() >= heatVentingCost)
        {
            switch (currentVentingTier)
            {
                case 0:
                    UpgradeHeatVenting();
                    break;
                case 1:
                    UpgradeHeatVenting();
                    break;
                case 2:
                    UpgradeHeatVenting();
                    UpdatedPurchaseText(heatVentingText);
                    break;
                default:
                    break;
            }
        }
        else
        {
            //Error Sound
        }
    }

    private void UpgradeHeatVenting()
    {
        player.SetHeatCooldown(heatVentingRate += heatVentingRateIncriment);
        heatVentingCost += heatVentingIncriment;
        currentVentingTier++;
        gameSession.UpdateScore(-heatVentingCost);
    }
}
