using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;
using TheBigBanger.PlayerStatics;
using TheBigBanger.PlayerInputSystems;

namespace TheBigBanger.GameModes
{
    /*Functions and variables needed to define the rules and abilities for the two child game modes Scenario and FreeRoam. 
      Contains functionalities of abilities due to them being executed within a mode.*/
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

    public class GameModeBase : GameControlAbilities
    {
        public GameObject PlayerPlanet;
        public MissionType MissionType = MissionType.SwipeDirection;
        [HideInInspector]
        public bool bLaunched = false;

        protected Camera arCamera;
        //common variables and functions between Scenario and Free Roam
        //Spawn player Planet, EndScene, etc..

        protected GameModeBase(GameManager manager) : base (manager)
        {
            arCamera = manager.ARCamera;
            PlayerPlanet = manager.PlayerGameObject;
        }

        public void SetARCamera()
        {
            if (arCamera == null)
                arCamera = GameObject.Find("/AR Session Origin/AR Camera").GetComponent<Camera>();
        }

        public void Feedback() 
        {
            if (MissionType == MissionType.SwipeDirection)
            {
                switch (TouchInput.GetTouch().phase)
                {
                    case TouchPhase.Began:
                        aSwipeMovement.StartSwipeLine();
                        break;
                    case TouchPhase.Moved:
                        aSwipeMovement.UpdateSwipeLine();
                        break;
                    case TouchPhase.Ended:
                        aSwipeMovement.EndSwipeLine();
                        break;
                }
            }
        }

        public void FreezeTime()
        {
            PlayerTime.bFreeze = true;
        }

        public void UnfreezeTime()
        {
            PlayerTime.bFreeze = false;
        }

        public Vector3 GetCameraDistanceToPlayer() 
        {
            return arCamera.transform.position;
        }
    }
}
