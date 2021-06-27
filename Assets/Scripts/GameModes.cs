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
        LevelStart,
        PlayPhase,
        LevelEnd,
    }

    public class GameMode : PlayerAbilityList
    {
        
        GameModeType gameModeType;
        GameObject playerPlanet, targetPlanet;
        GameObject placementIndicator, obstacle;
        public GamePhase gamePhase = GamePhase.SelectPlane;
        public string actionNeededText;
        public bool bLaunched = false, bTimeOver = false;
        public float bTimeLimit = 200f;
        Text debugText;
        public bool levelEnd = false;

        protected Camera arCamera;
        //common variables and functions between Scenario and Free Roam
        //Spawn player Planet, EndScene, etc..     
        protected GameMode(GameManager manager) : base (manager)
        {
            placementIndicator = GameObject.Find("PlacementIndicator");
            playerPlanet = manager.playerGameObject;
            targetPlanet = manager.targetGameObject;
            obstacle = manager.obstaclePrefab;
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
                if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Ended))
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

            else if (gamePhase == GamePhase.SpawnPhase)
            {
                Vector3 spawnPosition = placementIndicator.transform.position;
                playerPlanet.transform.position = new Vector3(spawnPosition.x - 0.25f, spawnPosition.y, spawnPosition.z);
                targetPlanet.transform.position = new Vector3(spawnPosition.x + 0.25f, spawnPosition.y, spawnPosition.z);
                SetPlanets(true);
                UnfreezeTime();
                gamePhase = GamePhase.LevelStart;
                actionNeededText = "";
            }

            else if (gamePhase == GamePhase.LevelStart)
            {
                actionNeededText = "place your obstacle on the play area";

                if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Ended))
                {
                    if (placementIndicator.activeSelf)
                    {
                        //GameObject.Instantiate(obstacle, placementIndicator.transform.position, Quaternion.identity);
                        gamePhase = GamePhase.PlayPhase;
                    }
                }
            }

            else if (gamePhase == GamePhase.PlayPhase && !bLaunched)
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

            else if (gamePhase == GamePhase.LevelEnd)
            {
                levelEnd = true;
            }
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

        //only for lesson units, not free roam
        public bool IsTimeOver()
        {
            return GameTime.gameTime > bTimeLimit;
        }

        //spawn stuff
        public void Reset() 
        {
            debugText.text = "RESET"; 
            playerPlanet.transform.position = playerPlanet.GetComponent<PAMovement>().startPos;
            targetPlanet.transform.position = targetPlanet.GetComponent<PBMovement>().startPos;
            aSwipeMovement.ResetSwipeLine();
            playerPlanet.GetComponent<PAMovement>().bIsMoving = false;
            bLaunched = false;
            levelEnd = false;
            gamePhase = GamePhase.PlayPhase;
        }
    }
}
