using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StarNavigation : MonoBehaviour
{
    [SerializeField] string levelToLoad;

    //tooltip Manager
    GameObject tooltip;

    private void Start()
    {
        FindTooltipManager();
    }

    private void FindTooltipManager()
    {
        tooltip = FindObjectOfType<TooltipManager>().ReturnTooltip();
    }

    //When the mouse enters the collider
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FindObjectOfType<SceneLoader>().LoadPassedScene(levelToLoad);
        }

        //Shows tooltip
        DisplayTooltip(true);
    }

    //When the mouse exits the collider
    private void OnMouseExit()
    {
        //Hides tooltip
        DisplayTooltip(false);
    }

    private void DisplayTooltip(bool display)
    {
        //Displays Tooltip
        tooltip.SetActive(display);
        //Sets the value that the tooltip will be raised on the Y axis
        float offsetY = FindObjectOfType<TooltipManager>().ReturnPadding();
        //Creates a vector2 and adds the offset on the Y axis
        Vector2 offset = new Vector2(transform.position.x, transform.position.y + offsetY);
        //Sets the position of the tooltip to this gameobjects position converted into canvas coordinates
        tooltip.transform.position = Camera.main.WorldToScreenPoint(offset);
        //Updates the text on the tooltip to display the level
        tooltip.GetComponentInChildren<TMPro.TMP_Text>().text = levelToLoad;
    }
}
