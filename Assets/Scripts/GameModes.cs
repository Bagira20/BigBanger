using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;
using TheBigBanger.PlayerStatics;
using TheBigBanger.PlayerInputSystems;

namespace TheBigBanger.GameModes {
    
    public enum GameModeType 
    {
        Scenario,
        FreeRoam
    }
    
    public class GameModeBase : MonoBehaviour 
    {
        protected Camera arCamera;
        public bool bLaunched = false;
        //common variables and functions between Scenario and Free Roam
        //Spawn player Planet, EndScene, etc..

        public void SetARCamera()
        {
            if (arCamera == null)
                arCamera = GameObject.Find("/AR Session Origin/AR Camera").GetComponent<Camera>();
        }
    }

    public class GameModeScenario : GameModeBase
    {
        public GameModeScenario(GameManager manager) 
        {
            arCamera = manager.ARCamera;
        }

        public void FreezeTime() 
        {
            PlayerTime.bGamePaused = true;
        }

        public void UnfreezeTime()
        {
            PlayerTime.bGamePaused = false;
        }

        public void UpdateSwipeDirection() 
        {
            if (TouchInput.RaycastFromCamera(arCamera))
            {
                //WIP size increase only as testing purposes!
                if (TouchInput.IsPlayerHit()) {
                    GameObject targetObject = TouchInput.GetHitObject();
                    targetObject.transform.localScale += Vector3.one;
                }
            }
        }
    }
}
