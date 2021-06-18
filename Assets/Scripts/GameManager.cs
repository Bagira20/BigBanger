using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheBigBanger.PlayerStatics;
using TheBigBanger.GameModes;
using TheBigBanger.PlayerInputSystems;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class GameManager : PlayerStaticsManager
{
    public GameModeType gameMode = GameModeType.Scenario;
    GameModeScenario activeScenario;
    //GameModeFreeRoam activeFreeRoam;
    public Camera ARCamera;
    public Text TimerText;

    void Awake()
    {
        SetGameMode();
    }

    public void SetGameMode() 
    {
        switch (gameMode)
        {
            case GameModeType.Scenario:
                activeScenario = new GameModeScenario(this);
                break;
            /*case GameModeType.FreeRoam:
                activeFreeRoam = new GameModeFreeRoam();
                break;*/
        }
    }

    void Update()
    {
        UpdateTime();
        UpdatePlayerTouchInput();
        UpdateGameMode();
        UpdateUICanvas();
    }

    void UpdateUICanvas() 
    {
        TimerText.text = "GameTime: " + PlayerTime.gameTime;
    }

    void UpdateGameMode() 
    {
        switch (gameMode)
        {
            case GameModeType.Scenario:
            {
                UpdateScenario();
                break;
            }
        }
    }

    void UpdateScenario() 
    {
        if (TouchInput.IsTouching())
        {
            if (TouchInput.RaycastFromCamera(ARCamera)) {
                if (!activeScenario.bLaunched && !PlayerTime.bGamePaused && TouchInput.IsPlayerHit())
                    activeScenario.FreezeTime();
                activeScenario.UpdateSwipeDirection(); 
            }
        }
    }

    public void LaunchButton() 
    {
        if (gameMode == GameModeType.Scenario)
        {
            //wip
            GameObject.Find("/Canvas/LaunchButton/Text").GetComponent<Text>().text = "launched!";
            activeScenario.bLaunched = true;
            PlayerTime.bGamePaused = false;
            activeScenario.UnfreezeTime();
        }
    }
}
