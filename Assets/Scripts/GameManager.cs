using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheBigBanger.PlayerStatics;
using TheBigBanger.GameModes;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;

public class GameManager : GameplayStaticsManager
{
    /*GameManager which should manage the Session across scenes, hold references to gameobjects within the scene, host GameModes and UI (plus interaction between GameMode and UI)*/

    [Header("Scene Settings")]
    public GameModeType gameMode = GameModeType.Lesson;

    [Header("Scene Objects")]
    public GameObject playerGameObject;
    public Camera arCamera;
    public Text timerText;
    public TMP_Text actionNeededText; 

    [Header("DEVELOPMENT Only")]
    public Text DebugText;

    ModeLesson _activeLesson;
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
            case GameModeType.Lesson:
                _activeLesson = new ModeLesson(this);
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

    void UpdateLessonUICanvas() 
    {
        timerText.text ="GameTime: " + (_activeLesson.bTimeLimit-Mathf.Round(GameTime.gameTime));
        actionNeededText.text = _activeLesson.actionNeededText;
    }

    void UpdateFreeRoamUICanvas()
    {
        actionNeededText.text = _activeFreeRoam.actionNeededText;
    }


    void UpdateGameMode() 
    {
        switch (gameMode)
        {
            case GameModeType.Lesson:
            {
                    UpdateLesson();
                    UpdateLessonUICanvas();
                    break;
            }
            case GameModeType.FreeRoam:
            {
                    UpdateFreeRoamUICanvas();
                    break;
            }

        }
    }

    void UpdateLesson() 
    {
        if (TouchInput.IsTouching())
        {
            if (TouchInput.RaycastFromCamera(arCamera)) 
            {
                if (!_activeLesson.bLaunched && !GameTime.bFreeze && TouchInput.IsPlayerHit())
                   _activeLesson.FreezeTime();

                _activeLesson.Feedback();
            }
        }
    }

    public void LaunchButton() 
    {
        if (gameMode == GameModeType.Lesson)
        {
            //wip
            GameObject.Find("/Canvas/LaunchButton/Text").GetComponent<Text>().text = "launched!";
            _activeLesson.bLaunched = true;
            GameTime.bFreeze = false;
            _activeLesson.UnfreezeTime();
        }
    }
}
