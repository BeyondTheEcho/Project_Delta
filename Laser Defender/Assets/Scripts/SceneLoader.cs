using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Awake()
    {
        //Set screen size for Standalone
            Screen.SetResolution(750, 1200, false);
            Screen.fullScreen = true;
    }

    //Loads the next scene in the build index
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    // Loads Scene Index zero
    public void BackToStart()
    {
        SceneManager.LoadScene(0);
    }

    //Quits to Desktop
    public void QuitButton()
    {
        Application.Quit();
    }

    //Public class that is called elsewhere and is passed a delay.
    public void LoadGameOver(int delay)
    {
        //Passes the delay into the Coroutine and starts it
        StartCoroutine(LoadAfterDelay(delay));
    }

    //Coroutine that is passed a delay when called
    public IEnumerator LoadAfterDelay(int delay)
    {
        //Waits for the delay that was passed to LoadGameOver()
        yield return new WaitForSeconds(delay);
        //Loads the Game Over scene
        SceneManager.LoadScene("Game Over");
        //Kills the coroutine
        StopCoroutine(LoadAfterDelay(delay));
    }
}
