using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheBigBanger.PlayerInputSystems;

public class AbilitySwipeMovement : AbilityBase
{
    public bool bPredictionInstantiated;
    public GameObject predictionLineGO, predictionLineCursor;
    public LineRenderer predictionLineRenderer;
    public Vector3[] linePositions;
    public AbilitySwipeMovement(GameManager manager) : base(manager) {}

    /*Ability: Swipe-Direction*/
    public void StartSwipeLine()
    {
        if (TouchInput.RaycastFromCamera(arCamera))
        {
            if (TouchInput.IsPlayerHit())
            {
                if (!bPredictionInstantiated)
                {
                    InitiateLineRenderer();
                    linePositions = new Vector3[] { PlayerPlanet.transform.position, predictionLineCursor.transform.position };
                    predictionLineRenderer.SetPositions(linePositions);
                }
                else
                    UpdateSwipeLine();
            }
        }
    }

    void InitiateLineRenderer()
    {
        predictionLineGO = new GameObject();
        predictionLineRenderer = predictionLineGO.AddComponent<LineRenderer>();
        predictionLineRenderer.useWorldSpace = false;
        predictionLineRenderer.materials[0] = new Material(Shader.Find("Default-Line"));
        bPredictionInstantiated = true;
    }

    public void UpdateSwipeLine()
    {
        //linePositions[1] = predictionLineCursor.transform.position;/*arCamera.ScreenToWorldPoint(PlayerInputPositions.currentTouchPos)+GetCameraDistanceToPlayer();*/
        predictionLineRenderer.SetPositions(linePositions);
    }

    public void EndSwipeLine()
    {

    }
}
