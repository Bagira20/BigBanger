using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySwipeMovement : AbilityBase
{
    public bool bPredictionInstantiated = false;
    public GameObject predictionCursor;
    public LineRenderer predictionLineRenderer;
    public Vector3 targetPosition, swipeDirection;
    public float swipeMagnitude;

    public AbilitySwipeMovement(GameManager manager) : base(manager) 
    {
        predictionLineRenderer = manager.playerGameObject.GetComponentInChildren<LineRenderer>();
        predictionLineRenderer.enabled = false;
    }

    /*Ability: Swipe-Direction*/
    public void StartSwipeLine()
    {
        if (TouchInput.RaycastFromCamera(arCamera))
        {
            if (!bPredictionInstantiated && TouchInput.IsPlayerHit())
            {
                InitiateLineRenderer();
                SetLineEndToTouchPosition();
            }
            else if (bPredictionInstantiated && (TouchInput.IsPlayerHit() || TouchInput.IsInputCanvasHit()))
                UpdateSwipeLine();
        }
    }

    void InitiateLineRenderer()
    {
        predictionLineRenderer.useWorldSpace = true;
        predictionLineRenderer.positionCount = 2;
        predictionLineRenderer.SetPosition(0, PlayerPlanet.transform.position);
        predictionLineRenderer.enabled = true;
        bPredictionInstantiated = true;
    }

    void SetLineEndToTouchPosition() 
    {
        targetPosition = TouchInput.GetHitWorldPosition();
        predictionLineRenderer.SetPosition(1, targetPosition);
    }

    public void UpdateSwipeLine()
    {
        if (TouchInput.RaycastFromCamera(arCamera))
        {
            if (TouchInput.IsInputCanvasHit() || TouchInput.IsPlayerHit())
            {
                SetLineEndToTouchPosition();
            }
        }
        UpdateSwipeData();
    }

    void UpdateSwipeData() 
    {
        Vector3 delta = PlayerPlanet.transform.position - targetPosition;
        swipeMagnitude = delta.magnitude;
        swipeDirection = delta / swipeMagnitude;
    }

    public void EndSwipeLine()
    {

    }
}
