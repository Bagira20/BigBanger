using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheBigBanger.GameStatics
{
    public class GameStaticsManager : MonoBehaviour
    {
        protected void UpdateGameTime()
        {
            if (GameTime.bGamePaused)
            {
                GameTime.levelTime = Time.timeSinceLevelLoad;
                GameTime.deltaTime = Time.deltaTime;
                GameTime.gameTime = Time.time;
            }
            else
                GameTime.deltaTime = 0.0f;
        }
    }

    struct GameTime
    {
        public static float gameTime = 0.0f, levelTime = 0.0f, deltaTime = 0.0f;
        public static bool bGamePaused = false;
    }
}