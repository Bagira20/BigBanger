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
    /*GameManager which should manage the Session across scenes, host GameModes and UI (plus interaction between GameMode and UI)*/

    public GameModeType gameMode = GameModeType.Scenario;
    public GameObject PlayerGameObject, lineCursor;
    ModeScenario activeScenario;
    ModeFreeRoam activeFreeRoam;
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
                activeScenario = new ModeScenario(this);
                break;
            case GameModeType.FreeRoam:
                activeFreeRoam = new ModeFreeRoam(this);
                break;
        }
    }

    void Update()
    {
        UpdateTime();
        UpdatePlayerTouchInput();
        UpdateGameMode();
    }

    void UpdateUICanvasTime() 
    {
        TimerText.text = "GameTime: " + (activeScenario.bTimeLimit-Mathf.Round(PlayerTime.gameTime));
    }

    void UpdateGameMode() 
    {
        switch (gameMode)
        {
            case GameModeType.Scenario:
            {
                UpdateScenario();
                UpdateUICanvasTime();
                break;
            }
        }
    }

    void UpdateScenario() 
    {
        if (TouchInput.IsTouching())
        {
            if (TouchInput.RaycastFromCamera(ARCamera)) 
            {
                if (!activeScenario.bLaunched && !PlayerTime.bFreeze && TouchInput.IsPlayerHit())
                   activeScenario.FreezeTime();

                activeScenario.Feedback();
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
            PlayerTime.bFreeze = false;
            activeScenario.UnfreezeTime();
        }
    }
}
