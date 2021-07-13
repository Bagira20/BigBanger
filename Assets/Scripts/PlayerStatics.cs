using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheBigBanger.GameplayStatics
{
    /*Library of functions and values regarding player info (time, input, etc.) that should be accessible from every class. 
      Parent of GameManager*/

    public class GameplayStaticsManager : MonoBehaviour
    {
        protected void UpdateTime()
        {
            if (GameTime.bTimeStarted)
            {
                GameTime.gameTime += Time.deltaTime;
                if (!GameTime.bFreeze)
                {
                    GameTime.deltaTime = Time.deltaTime;
                    GameTime.unfrozenTime += Time.deltaTime;
                }
                else
                    GameTime.deltaTime = 0.0f;
            }
        }

        public void StartTime() 
        {
            GameTime.bTimeStarted = true;
        }

        public void StopTime() 
        {
            GameTime.bTimeStarted = false;
        }

        protected void UpdatePlayerTouchInput()
        {
            if (TouchInput.IsTouching())
            {
                switch (TouchInput.GetTouch().phase) 
                {
                    case TouchPhase.Began:
                        PlayerInputPositions.startTouchPos = TouchInput.GetTouchPosition(); break;
                    case TouchPhase.Moved:
                        PlayerInputPositions.currentTouchPos = TouchInput.GetTouchPosition(); break;
                    case TouchPhase.Ended:
                        PlayerInputPositions.endTouchPos = TouchInput.GetTouchPosition(); break;
                }
            }
            else
                PlayerInputPositions.startTouchPos = PlayerInputPositions.currentTouchPos = PlayerInputPositions.endTouchPos = Vector2.zero;
        }
    }

    struct GameTime
    {
        public static float unfrozenTime = 0.0f, gameTime = 0.0f, deltaTime = 0.0f;
        public static bool bFreeze = true, bTimeStarted = false;
    }

    struct PlayerInputPositions
    {
        public static Vector2 startTouchPos, currentTouchPos, endTouchPos;
    }

    struct PlayerData
    {
        public static string playerName = "Player";
        public static int playerProgress = 0;
    }
}