using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBase : MonoBehaviour
{
    /*Base for single abilities of player control, e.g., swipe movement, rocket mode, etc.*/
    protected Camera arCamera;
    protected GameManager gameManager;
    protected GameObject playerPlanet;
    protected PAMovement playerMovement;
    public GameObject inputCursor;
    public LineRenderer predictionLine;
    public bool initialized = false;
    public static bool bInputCursorCreated = false;
    static string touchCursorName = "TouchInputCursor", predictionLineName = "PredictionLine";

    public AbilityBase(GameManager manager)
    {
        gameManager = manager;
        arCamera = manager.arCamera;
        playerPlanet = manager.playerGameObject;
        playerMovement = playerPlanet.GetComponent<PAMovement>();
        if (!AbilityBase.bInputCursorCreated)
        {
            inputCursor = new GameObject();
            inputCursor.name = touchCursorName;
            bInputCursorCreated = true;
            predictionLine = Instantiate(playerMovement.lineRenderer);
            predictionLine.name = predictionLineName;
            predictionLine.enabled = false;
            predictionLine.widthCurve = playerMovement.lineWidthCurve;
            predictionLine.widthMultiplier = playerMovement.lineWidthMultiplier;
        }
        else
        {
            inputCursor = GameObject.Find(touchCursorName);
            predictionLine = GameObject.Find(predictionLineName).GetComponent<LineRenderer>();
        }
    }
}