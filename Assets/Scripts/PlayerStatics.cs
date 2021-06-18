using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheBigBanger.PlayerInputSystems;

namespace TheBigBanger.PlayerStatics
{
    /*Library of functions and values regarding player info (time, input, etc.) that should be accessible from every class. 
      Parent of GameManager*/

    public class PlayerStaticsManager : MonoBehaviour
    {
        protected void UpdateTime()
        {
            if (!PlayerTime.bFreeze)
            {
                PlayerTime.deltaTime = Time.deltaTime;
                PlayerTime.gameTime += Time.deltaTime;
            }
            else
                PlayerTime.deltaTime = 0.0f;
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

    struct PlayerTime
    {
        public static float gameTime = 0.0f, deltaTime = 0.0f;
        public static bool bFreeze = false;
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