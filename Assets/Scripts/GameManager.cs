using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheBigBanger.PlayerStatics;
using TheBigBanger.GameModes;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class GameManager : GameplayStaticsManager
{
    /*GameManager which should manage the Session across scenes, hold references to gameobjects within the scene, host GameModes and UI (plus interaction between GameMode and UI)*/

    [Header("Scene Settings")]
    public GameModeType gameMode = GameModeType.Scenario;

    [Header("Scene Objects")]
    public GameObject playerGameObject;
    public Camera arCamera;
    public Text timerText;

    [Header("DEVELOPMENT Only")]
    public Text DebugText;

    ModeLevel _activeScenario;
    ModeFreeRoam _activeFreeRoam;

    void Awake()
    {
        SetGameMode();
        ARLibrary.Initialize();
    }

    public void SetGameMode() 
    {
        switch (gameMode)
        {
            case GameModeType.Scenario:
                _activeScenario = new ModeLevel(this);
                break;
            case GameModeType.FreeRoam:
                _activeFreeRoam = new ModeFreeRoam(this);
                break;
        }
    }

    void Update()
    {
        UpdateTime();
        UpdatePlayerTouchInput();
        UpdateGameMode();
        ARLibrary.UpdateARLibrary();
    }

    void UpdateUICanvasTime() 
    {
        timerText.text ="GameTime: " + (_activeScenario.bTimeLimit-Mathf.Round(GameTime.gameTime));
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
            if (TouchInput.RaycastFromCamera(arCamera)) 
            {
                if (!_activeScenario.bLaunched && !GameTime.bFreeze && TouchInput.IsPlayerHit())
                   _activeScenario.FreezeTime();

                _activeScenario.Feedback();
            }
        }
    }

    public void LaunchButton() 
    {
        if (gameMode == GameModeType.Scenario)
        {
            //wip
            GameObject.Find("/Canvas/LaunchButton/Text").GetComponent<Text>().text = "launched!";
            _activeScenario.bLaunched = true;
            GameTime.bFreeze = false;
            _activeScenario.UnfreezeTime();
        }
    }
}
