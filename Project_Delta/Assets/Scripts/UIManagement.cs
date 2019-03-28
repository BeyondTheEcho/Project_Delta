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

    //Stored References
    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        FindGameSession();
    }

    private void FindGameSession()
    {
        gameSession = FindObjectOfType<GameSession>();
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
            Time.timeScale = .0000001f;
            upgradeCanvas.SetActive(true);
        }
        else if (isPaused == false)
        {
            Time.timeScale = 1.0f;
            upgradeCanvas.SetActive(false);
        }
    }

    public void DoubleLasers()
    {
        if (doubleLasers == false)
        {
            if(gameSession.GetScore() >= doubleLasersCost)
            {
                doubleLasers = true;
                doubleLasersText.GetComponent<TMP_Text>().color = Color.green;
                doubleLasersText.GetComponent<TMP_Text>().text = purchasedText;
            }
            else
            {
                //Play Error Sound
            }
        }
    }
}
