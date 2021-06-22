using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;
using TheBigBanger.PlayerStatics;

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

    public class GameModeBase : PlayerAbilityList
    {
        public GameObject playerPlanet, targetPlanet;
        public MissionType missionType = MissionType.SwipeDirection;
        [HideInInspector]
        public bool bLaunched = false;
        Text debugText;

        protected Camera arCamera;
        //common variables and functions between Scenario and Free Roam
        //Spawn player Planet, EndScene, etc..

        protected GameModeBase(GameManager manager) : base (manager)
        {
            arCamera = manager.arCamera;
            playerPlanet = manager.playerGameObject;
            debugText = manager.DebugText;
        }

        public void SetARCamera()
        {
            if (arCamera == null)
                arCamera = GameObject.Find("/AR Session Origin/AR Camera").GetComponent<Camera>();
        }

        public void Feedback() 
        {
            if (missionType == MissionType.SwipeDirection)
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
            debugText.text = "Magnitude: " + aSwipeMovement.swipeMagnitude + "\n"+"Direction: " + aSwipeMovement.swipeDirection;
        }

        public void FreezeTime()
        {
            GameTime.bFreeze = true;
        }

        public void UnfreezeTime()
        {
            GameTime.bFreeze = false;
        }

        //spawn stuff
    }
}
