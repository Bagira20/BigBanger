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
    public LineRenderer predictionLineRenderer;
    public bool initialized = false;
    public static bool bInputCursorCreated = false;
    static string touchCursorName = "TouchInputCursor", predictionLineName="PredictionLine";

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
            predictionLineRenderer = Instantiate(manager.playerGameObject.GetComponent<PAMovement>().lineRenderer);
            predictionLineRenderer.name = predictionLineName;
            predictionLineRenderer.enabled = false;
        }
        else
        {
            inputCursor = GameObject.Find(touchCursorName);
            predictionLineRenderer = GameObject.Find(predictionLineName).GetComponent<LineRenderer>();
        }
    }
}