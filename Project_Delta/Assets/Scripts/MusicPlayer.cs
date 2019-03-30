using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    //Awake is called before ANYTHING else.
    void Awake()
    {
        //If you are unfamiliar with a Singleton pattern or the Unity Script Lifecycle Flowchart(Link Before), googling may be in order
        //https://docs.unity3d.com/Manual/ExecutionOrder.html
        Singleton(); 
    }

    private void Singleton()
    {
        //On Awake(Before ANYTHING  else), check to see if there is already an object of this type
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            //If there is, then this must be the duplicate, destroy this game object
            Destroy(gameObject);
        }
        else
        {
            //If there is no other object of this type, Do not destroy this object on load
            DontDestroyOnLoad(gameObject);
        }
    }
}
