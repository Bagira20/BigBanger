using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySwipeMovement : AbilityBase
{
    public bool bPredictionInstantiated = false;
    public Vector3 targetPosition, swipeDirection;
    public float swipeMagnitude;

    public AbilitySwipeMovement(GameManager manager) : base(manager) 
    {
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
        predictionLine.useWorldSpace = true;
        predictionLine.positionCount = 2;
        predictionLine.enabled = true;
        bPredictionInstantiated = true;
        SetLinePositions(TouchInput.GetHitWorldPositionAtLayer(3));
    }

    public void SetLinePositions(Vector3 newEndPosition) 
    {
        predictionLine.SetPosition(0, playerPlanet.transform.position);
        targetPosition = newEndPosition;
        predictionLine.SetPosition(1, targetPosition);
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

    public void UpdateSwipeData() 
    {
        Vector3 delta = targetPosition - playerPlanet.transform.position;
        swipeMagnitude = delta.magnitude;
        swipeDirection = delta / swipeMagnitude;
        inputCursor.transform.position = targetPosition;
    }

    public void EndSwipeLine()
    {

    }

    public void ResetSwipeLine() 
    {
        initialized = false;
        predictionLine.enabled = false;
        bPredictionInstantiated = false;
    }
}