using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;
using TheBigBanger.PlayerStatics;
using TheBigBanger.PlayerInputSystems;

namespace TheBigBanger.GameModes
{
    /*Functions and variables needed to define the rules and abilities within the two game modes Scenario and FreeRoam. 
      GameModeBase includes functions for both modes.*/
    public enum GameModeType
    {
        Scenario,
        FreeRoam
    }

    public enum MissionType
    {
        SwipeDirection,
        Rocket,
        Everything,
        None
    }

    public class GameModeBase : MonoBehaviour
    {
        public GameObject PlayerPlanet;
        public MissionType MissionType;
        [HideInInspector]
        public bool bLaunched = false;

        protected bool bPredictionInstantiated;
        protected GameObject predictionLineGO;
        protected LineRenderer predictionLineRenderer;
        protected Vector3[] linePositions;
        protected Camera arCamera;
        //common variables and functions between Scenario and Free Roam
        //Spawn player Planet, EndScene, etc..

        protected GameModeBase(GameManager manager)
        {
            arCamera = manager.ARCamera;
            PlayerPlanet = manager.PlayerGameObject;
        }

        public void SetARCamera()
        {
            if (arCamera == null)
                arCamera = GameObject.Find("/AR Session Origin/AR Camera").GetComponent<Camera>();
        }

        public void FreezeTime()
        {
            PlayerTime.bFreeze = true;
        }

        public void UnfreezeTime()
        {
            PlayerTime.bFreeze = false;
        }


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
                        linePositions = new Vector3[] { PlayerPlanet.transform.position, PlayerPlanet.transform.position + Vector3.one };
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
            linePositions[1] = arCamera.ScreenToWorldPoint(PlayerInputPositions.currentTouchPos)+GetCameraDistanceToPlayer();
            predictionLineRenderer.SetPositions(linePositions);
        }

        public void EndSwipeLine()
        {

        }

        public Vector3 GetCameraDistanceToPlayer() 
        {
            return PlayerPlanet.transform.position - arCamera.transform.position;
        }
    }

    public class GameModeScenario : GameModeBase
    {
        public float bTimeLimit = 200f;
        public bool bTimeOver = false;

        public GameModeScenario(GameManager manager) : base(manager) { }

        public bool IsTimeOver() 
        {
            return PlayerTime.gameTime > bTimeLimit;
        }
    }

    public class GameModeFreeRoam : GameModeBase 
    {
        public GameModeFreeRoam(GameManager manager) : base(manager) { }
    }
}
