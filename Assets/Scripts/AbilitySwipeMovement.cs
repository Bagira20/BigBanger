using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySwipeMovement : AbilityBase
{
    public bool bPredictionInstantiated = false;
    public LineRenderer predictionLineRenderer;
    public Vector3 targetPosition, swipeDirection;
    public float swipeMagnitude;


    public AbilitySwipeMovement(GameManager manager) : base(manager) 
    {
        predictionLineRenderer = Instantiate(manager.playerGameObject.GetComponent<PAMovement>().lineRenderer);
        predictionLineRenderer.enabled = false;
    }

    /*Ability: Swipe-Direction*/
    public void StartSwipeLine()
    {
        if (!TouchInput.IsUIHit())
        {
            if (!bPredictionInstantiated && TouchInput.IsPlayerHit())
                InitiateLineRenderer();
            else if (bPredictionInstantiated && (TouchInput.IsPlayerHit() || TouchInput.IsInputCanvasHit()))
                UpdateSwipeLine();
        }
    }

    void InitiateLineRenderer()
    {
        gameManager.activeMode.FreezeTime();
        initialized = true;
        predictionLineRenderer.useWorldSpace = true;
        predictionLineRenderer.positionCount = 2;
        predictionLineRenderer.enabled = true;
        bPredictionInstantiated = true;
        SetLinePositions(TouchInput.GetHitWorldPositionAtLayer(3));
    }

    void SetLinePositions(Vector3 newEndPosition) 
    {
        predictionLineRenderer.SetPosition(0, PlayerPlanet.transform.position);
        targetPosition = newEndPosition;
        predictionLineRenderer.SetPosition(1, targetPosition);
    }

    public void UpdateSwipeLine()
    {
        if (!TouchInput.IsUIHit())
        {
            if (TouchInput.IsInputCanvasHit() || TouchInput.IsPlayerHit())
            {
                SetLinePositions(TouchInput.GetHitWorldPositionAtLayer(3));
            }
        }
        UpdateSwipeData();
    }

    void UpdateSwipeData() 
    {
        Vector3 delta = targetPosition - PlayerPlanet.transform.position;
        swipeMagnitude = delta.magnitude;
        swipeDirection = delta / swipeMagnitude;
    }

    public void EndSwipeLine()
    {

    }

    public void ResetSwipeLine() 
    {
        initialized = false;
        predictionLineRenderer.enabled = false;
        bPredictionInstantiated = false;
    }
}