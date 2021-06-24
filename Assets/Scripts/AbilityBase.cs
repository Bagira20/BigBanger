using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBase : MonoBehaviour
{
    /*Base for single abilities of player control, e.g., swipe movement, rocket mode, etc.*/
    protected Camera arCamera;
    protected GameObject PlayerPlanet;
    public bool initialized = false;

    public AbilityBase(GameManager manager) 
    {
        arCamera = manager.arCamera;
        PlayerPlanet = manager.playerGameObject;
    }
}
