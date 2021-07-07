using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBase : MonoBehaviour
{
    /*Base for single abilities of player control, e.g., swipe movement, rocket mode, etc.*/
    protected Camera arCamera;
    protected GameManager gameManager;
    protected GameObject PlayerPlanet;
    public GameObject inputCursor;
    public bool initialized = false;
    public static bool bInputCursorCreated = false;
    static string touchCursorName = "TouchInputCursor";

    public AbilityBase(GameManager manager)
    {
        gameManager = manager;
        arCamera = manager.arCamera;
        PlayerPlanet = manager.playerGameObject;
        if (!AbilityBase.bInputCursorCreated)
        {
            inputCursor = new GameObject();
            inputCursor.name = touchCursorName;
            bInputCursorCreated = true;
        }
        else
            inputCursor = GameObject.Find(touchCursorName);
    }
}