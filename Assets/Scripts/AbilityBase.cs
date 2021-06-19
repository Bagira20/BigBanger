using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBase : MonoBehaviour
{
    /*Base for single abilities of player control, e.g., swipe movement, rocket mode, etc.*/
    public Camera arCamera;
    public GameObject PlayerPlanet, LineRendererPrefab;

    public AbilityBase(GameManager manager) 
    {
        arCamera = manager.arCamera;
        PlayerPlanet = manager.playerGameObject;
    }
}
