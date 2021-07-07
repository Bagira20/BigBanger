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
        SpawnPhase,
        PlaceObstacles,
        PlayPhase,
        LevelEnd,
    }

    public class GameMode : PlayerAbilityList
    {
        GameModeType gameModeType;
        GameManager gameManager;
        GameObject playerPlanet, targetPlanet;
        PAMovement playerMovement;
        PBMovement targetMovement;
        GameObject placementIndicator, obstacle;
        public GamePhase gamePhase = GamePhase.SelectPlane, previousGamePhase;
        public string actionNeededText;
        public bool bLaunched = false, bTimeOver = false, levelEnd = false;
        public float bTimeLimit = 200f;
        Text debugText;

        protected Camera arCamera;
        //common variables and functions between Scenario and Free Roam
        //Spawn player Planet, EndScene, etc..     
        protected GameMode(GameManager manager) : base (manager)
        {
            gameManager = manager;
            placementIndicator = GameObject.Find("PlacementIndicator");
            playerPlanet = gameManager.playerGameObject;
            playerMovement = playerPlanet.GetComponent<PAMovement>();
            targetPlanet = gameManager.targetGameObject;
            targetMovement = targetPlanet.GetComponent<PBMovement>();
            obstacle = gameManager.obstaclePrefab;
            arCamera = gameManager.arCamera;
            gameModeType = gameManager.gameMode;
            debugText = gameManager.DebugText;
            gameManager.levelMissionCanvas.GetComponentInChildren<Text>().text = LevelIntroDisplays.LevelIntroText[1];
            SetPlanets(false);
        }

        public void SetARCamera()
        {
            if (arCamera == null)
                arCamera = GameObject.Find("/AR Session Origin/AR Camera").GetComponent<Camera>();
        }
        
        public void UpdateGameMode() 
        {
            switch (gamePhase) 
            {
                case GamePhase.SelectPlane:
                    UpdateSelectPlane(); break;

                case GamePhase.SpawnPhase:
                    UpdateSpawnPhase(); break;

                case GamePhase.PlaceObstacles:
                    UpdatePlaceObstacles(); break;

                case GamePhase.PlayPhase:
                    UpdatePlayPhase(); break;

                case GamePhase.LevelEnd:
                    UpdateLevelEnd(); break;
            };
        }

        void UpdateSelectPlane() 
        {
            actionNeededText = "move your device slowly until an indicator appears";
            if (TouchInput.IsTouching() && (Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                if (placementIndicator.activeSelf)
                {
                    SetGamePhase(GamePhase.SpawnPhase);
                }
                else
                {
                    actionNeededText = "Please select a valid surface to play on";
                }
            }
        }

        void UpdateSpawnPhase() 
        {
            Vector3 spawnPosition = placementIndicator.transform.position;
            playerPlanet.transform.position = new Vector3(spawnPosition.x - 0.25f, spawnPosition.y, spawnPosition.z);
            targetPlanet.transform.position = new Vector3(spawnPosition.x + 0.25f, spawnPosition.y, spawnPosition.z);
            SetPlanets(true);
            if (gameManager.ObstacleCreationAtStart)
            {
                SetGamePhase(GamePhase.PlaceObstacles);
                actionNeededText = "place your obstacle on the play area";
            }
            else
            {
                SetGamePhase(GamePhase.PlayPhase);
                UnfreezeTime();
            }
        }

        void UpdatePlaceObstacles()
        {  
            if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                if (placementIndicator.activeSelf)
                {
                    GameObject.Instantiate(obstacle, placementIndicator.transform.position, Quaternion.identity);
                    SetGamePhase(GamePhase.PlayPhase);
                    UnfreezeTime();
                }
            }
        }

        void UpdatePlayPhase()
        {
            //Check Time for else if = GAMEOVER
            if (IsTimeOver())
            {
                gameManager.levelEndCanvas.GetComponentInChildren<Text>().text = "Time ran out!";
                SetGamePhase(GamePhase.LevelEnd);
            }

            //Check Input
            if (!bLaunched)
            {
                if (TouchInput.IsTouching() && TouchInput.RaycastFromCamera(arCamera))
                {
                    if (playerMovement.movementInputType == PAMovementTypes.Swipe) 
                    {
                        if (TouchInput.IsRotationSocketHit())
                            UpdateRotationSocketInput();
                        else
                            UpdateSwipeInput();
                    }
                    else if (playerMovement.movementInputType == PAMovementTypes.Rocket) { /*ROCKET STUFF*/}
                }
                debugText.text = "PLAYER: \nVelocity: " + playerMovement.GetVelocityFromAbility(playerAbilities.swipeMovement) + "\nForce: " + playerMovement.GetForceFromAbility(playerAbilities.swipeMovement) + "\nMass: " + playerMovement.GetMass();
            }
        }

        void UpdateRotationSocketInput() 
        {
            switch (TouchInput.GetTouch().phase)
            {
                case TouchPhase.Began:
                    aRotation.StartRotation();
                    break;
                case TouchPhase.Moved:
                    aRotation.UpdateRotation();
                    break;
                case TouchPhase.Ended:
                    aRotation.EndRotation();
                    break;
            }
        }

        void UpdateSwipeInput() 
        {
            switch (TouchInput.GetTouch().phase)
            {
                case TouchPhase.Began:
                    aSwipeMovement.StartSwipeLine();
                    if (!IsTimeFrozen())
                        FreezeTime();
                    break;
                case TouchPhase.Moved:
                    aSwipeMovement.UpdateSwipeLine();
                    break;
                case TouchPhase.Ended:
                    aSwipeMovement.EndSwipeLine();
                    break;
            }
        }

        //called by collision between player and targetPlanet
        void UpdateLevelEnd() 
        {
            levelEnd = true;
        }

        void SetGamePhase(GamePhase newPhase) 
        {
            previousGamePhase = gamePhase;
            gamePhase = newPhase;
        }

        void SetPlanets(bool active) 
        {
            playerPlanet.SetActive(active);
            targetPlanet.SetActive(active);
            if (active) 
            {
                playerPlanet.GetComponent<PAMovement>().startPos = playerPlanet.transform.position;
                targetPlanet.GetComponent<PBMovement>().startPos = targetPlanet.transform.position;
            }
        }

        public void FreezeTime()
        {
            GameTime.bFreeze = true;
        }

        public void UnfreezeTime()
        {
            GameTime.bFreeze = false;
        }

        public bool IsTimeFrozen() 
        {
            return GameTime.bFreeze;
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
            playerPlanet.GetComponent<PAMovement>().ResetPlanet();
            if (levelEnd)
                targetPlanet.GetComponent<PBMovement>().ResetPlanet();
            aSwipeMovement.ResetSwipeLine();
            bLaunched = false;
            levelEnd = false;
            gamePhase = GamePhase.PlayPhase;
        }
    }
}
