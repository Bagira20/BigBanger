using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBase : MonoBehaviour
{
    public Camera arCamera;
    public GameObject PlayerPlanet;

    public AbilityBase(GameManager manager) 
    {
        arCamera = manager.ARCamera;
        PlayerPlanet = manager.PlayerGameObject;
    }
}
