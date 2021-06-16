using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatics : MonoBehaviour
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
