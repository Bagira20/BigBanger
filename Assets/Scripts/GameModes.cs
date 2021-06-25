using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;
using TheBigBanger.GameplayStatics;
using TMPro;

namespace TheBigBanger.GameModeManager
{
    /*Functions and variables needed to define the rules and abilities for the two child game modes Scenario and FreeRoam. 
      Contains functionalities of abilities due to them being executed within a mode.*/
    public enum GameModeType
    {
        Lesson,
        FreeRoam
    }

    public enum GamePhase
    {
        SelectPlane,
        SpawnPhase, //planets appear, planet values are give, bonus mission and formula appear and adjust on screen
        LevelStart,

        /// <summary>
        /// need to merge swipe and rocket into one phase
        /// </summary>
        SwipeDirection,
        Rocket,
    }

    public class GameMode : PlayerAbilityList
    {
        GameModeType gameModeType;
        GameObject playerPlanet, targetPlanet;
        public GamePhase gamePhase = GamePhase.SelectPlane;
        public string actionNeededText;
        public bool bLaunched = false, bTimeOver = false;
        public float bTimeLimit = 200f;
        Text debugText;

        protected Camera arCamera;
        //common variables and functions between Scenario and Free Roam
        //Spawn player Planet, EndScene, etc..     

        protected GameMode(GameManager manager) : base (manager)
        {
            arCamera = manager.arCamera;
            playerPlanet = manager.playerGameObject;
            gameModeType = manager.gameMode;
            debugText = manager.DebugText;
        }

        public void SetARCamera()
        {
            if (arCamera == null)
                arCamera = GameObject.Find("/AR Session Origin/AR Camera").GetComponent<Camera>();
        }

        public void Feedback() 
        {
            if (gamePhase == GamePhase.SelectPlane)
            {
                
            }
            /*if (gamePhase == GamePhase.SwipeDirection)
            {*/
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
            //}
            //if mission select plane ( plane mesh appears, set canvas text to "select a playable area") once selected by tapping spawn objects, switch mission to place obstacle

            //if spawn phase

            //if level start, timerfreeze =  false

        }

        public void FreezeTime()
        {
            GameTime.bFreeze = true;
        }

        public void UnfreezeTime()
        {
            GameTime.bFreeze = false;
        }

        public bool IsTimeOver()
        {
            return GameTime.gameTime > bTimeLimit;
        }

        //spawn stuff
    }
}
