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
        GameObject placementIndicator;
        public GamePhase gamePhase = GamePhase.SelectPlane;
        public string actionNeededText;
        public bool bLaunched = false, bTimeOver = false;
        public float bTimeLimit = 200f;
        Text debugText;

        protected Camera arCamera;
        Vector3 playerStartPos, targetStartPos;
        //common variables and functions between Scenario and Free Roam
        //Spawn player Planet, EndScene, etc..     

        protected GameMode(GameManager manager) : base (manager)
        {
            placementIndicator = GameObject.Find("PlacementIndicator");
            playerPlanet = manager.playerGameObject;
            targetPlanet = manager.targetGameObject;
            actionNeededText = "move your device slowly until an indicator appears";
            arCamera = manager.arCamera;
            gameModeType = manager.gameMode;
            debugText = manager.DebugText;
            SetPlanets(false);
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
                if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
                {
                    if (placementIndicator.activeSelf)
                    {
                        gamePhase = GamePhase.SpawnPhase;
                    }
                    else
                    {
                        actionNeededText = "Please select a valid surface to play on";
                    }
                }
            }

            if (gamePhase == GamePhase.SpawnPhase)
            {
                Vector3 spawnPosition = placementIndicator.transform.position;
                playerPlanet.transform.position = playerStartPos = new Vector3(spawnPosition.x - 0.25f, spawnPosition.y, spawnPosition.z);
                targetPlanet.transform.position = targetStartPos = new Vector3(spawnPosition.x + 0.25f, spawnPosition.y, spawnPosition.z);
                SetPlanets(true);
                gamePhase = GamePhase.LevelStart;
                actionNeededText = "";
            }

            if (gamePhase == GamePhase.LevelStart && !bLaunched)
            {
                actionNeededText = "";
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
                //DEBUG
                PAMovement tempMovement = playerPlanet.GetComponent<PAMovement>();
                debugText.text = "PLAYER: \nVelocity: " + tempMovement.GetVelocityFromAbility(playerAbilities.swipeMovement) + "\nForce: " + tempMovement.GetForceFromAbility(playerAbilities.swipeMovement) + "\nMass: " + tempMovement.GetMass();
                //}
            }

        }

        void SetPlanets(bool active) 
        {
            playerPlanet.SetActive(active);
            targetPlanet.SetActive(active);
        }

        public void FreezeTime()
        {
            GameTime.bFreeze = true;
        }

        public void UnfreezeTime()
        {
            GameTime.bFreeze = false;
        }

        //only for lesson units, not free roam
        public bool IsTimeOver()
        {
            return GameTime.gameTime > bTimeLimit;
        }

        //spawn stuff
        public void Reset() 
        {
            debugText.text = "RESET"; 
            playerPlanet.transform.position = playerStartPos;
            targetPlanet.transform.position = targetStartPos;
            aSwipeMovement.ResetSwipeLine();
            playerPlanet.GetComponent<PAMovement>().bIsMoving = false;
            bLaunched = false;
        }
    }
}
