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
    public enum EGameModeType
    {
        Lesson,
        FreeRoam
    }

    public enum EGamePhase
    {
        SelectPlane,
        SpawnPhase,
        PlaceObstacles,
        PlayPhase,
        LevelEnd,
    }

    public class GameMode : PlayerAbilityList
    {
        EGameModeType gameModeType;
        GameManager gameManager;
        GameObject playerPlanet, targetPlanet;
        public PAMovement playerMovement;
        PBMovement targetMovement;
        GameObject placementIndicator, obstacle;
        public EGamePhase gamePhase = EGamePhase.SelectPlane, previousGamePhase;
        public string actionNeededText;
        public bool bLaunched = false, bFirstLaunch = false, bTimeOver = false, levelEnd = false;
        public float bTimeLimit = 200f;
        public int gamepass = 0;
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
            gameModeType = gameManager.gameModeType;
            debugText = gameManager.DebugText;
            //gameManager.levelMissionCanvas.GetComponentInChildren<Text>().text = LevelIntroDisplays.LevelIntroText[1];
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
                case EGamePhase.SelectPlane:
                    UpdateSelectPlane(); break;

                case EGamePhase.SpawnPhase:
                    UpdateSpawnPhase(); break;

                case EGamePhase.PlaceObstacles:
                    UpdatePlaceObstacles(); break;

                case EGamePhase.PlayPhase:
                    UpdatePlayPhase(); break;

                case EGamePhase.LevelEnd:
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
                    gameManager.StartTime();
                    SetGamePhase(EGamePhase.SpawnPhase);
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
                SetGamePhase(EGamePhase.PlaceObstacles);
                actionNeededText = "place your obstacle on the play area";
            }
            else
            {
                gameManager.actionText.SetActive(false);
                SetGamePhase(EGamePhase.PlayPhase);
                gameManager.SetPlayPhaseUI(true);
                AudioPlayer.Play2DAudioFromRange(gameManager.activeMode.playerMovement.audioSource, gameManager.canvas.SelectSounds, new Vector2(0.8f, 1.2f), new Vector2(0.95f, 1.1f));
                UnfreezeTime();
                gameManager.StartTime();
            }
        }

        void UpdatePlaceObstacles()
        {  
            if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                if (placementIndicator.activeSelf)
                {
                    gameManager.actionText.SetActive(false);
                    GameObject.Instantiate(obstacle, placementIndicator.transform.position, Quaternion.identity);
                    SetGamePhase(EGamePhase.PlayPhase);
                    gameManager.SetPlayPhaseUI(true);
                    AudioPlayer.Play2DAudioFromRange(gameManager.activeMode.playerMovement.audioSource, gameManager.canvas.SelectSounds, new Vector2(0.8f, 1.2f), new Vector2(0.95f, 1.1f));
                    UnfreezeTime();
                }
            }
        }

        void UpdatePlayPhase()
        {
            //Check Time for else if = GAMEOVER
            if (IsTimeOver())
            {
                SetGamePhase(EGamePhase.LevelEnd);
            }

            //Check Input
            if (!bLaunched)
            {
                if (TouchInput.IsTouching() && TouchInput.RaycastFromCamera(arCamera))
                {
                    if (!TouchInput.IsPlayerHit() && (TouchInput.IsRotationSocketHit() || aRotation.bInputLocked))
                        UpdateRotationInputForAbility(aSwipeMovement);
                    else if ((TouchInput.IsPlayerHit() || TouchInput.IsInputCanvasHit()) && !TouchInput.IsUIHit())
                        UpdateSwipeInput();
                }
                // debugText.text = "PLAYER: \nVelocity: " + gameManager.GetTransformedValue(playerMovement.GetVelocityFromAbility(EPlayerAbilities.swipeMovement)) 
                //   + "\nForce: " + gameManager.GetTransformedValue(playerMovement.GetForceFromAbility(EPlayerAbilities.swipeMovement)) 
                // + "\nMass: " + gameManager.GetTransformedValue(playerMovement.GetMass());
                //target planet: define pos on first freeze as reset pos
            }
        }

        void UpdateRotationInputForAbility(AbilityBase ability) 
        {
            //case condition yet to do
            switch (TouchInput.GetTouch().phase)
            {
                case TouchPhase.Began:
                    aRotation.StartRotation(ability);
                    break;
                case TouchPhase.Moved:
                    aRotation.UpdateRotation(ability);
                    break;
                case TouchPhase.Ended:
                    aRotation.EndRotation(ability);
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
            gameManager.StopTime();
            levelEnd = true;
        }

        void SetGamePhase(EGamePhase newPhase) 
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
            return GameTime.unfrozenTime > bTimeLimit;
        }

        //spawn stuff
        public void Reset() 
        {
            playerPlanet.GetComponent<PAMovement>().ResetPlanet();
            targetPlanet.GetComponent<PBMovement>().ResetPlanet();
            aSwipeMovement.ResetSwipeLine();
            bFirstLaunch = false;
            bLaunched = false;
            levelEnd = false;
            gamePhase = EGamePhase.PlayPhase;
            aRocketControl.ResetRocket();
        }
    }
}
