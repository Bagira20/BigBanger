using System.Collections;
using System.Collections.Generic;
using TheBigBanger.GameplayStatics;
using TheBigBanger.GameModeManager;
using TheBigBanger.Formulae;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;

public class GameManager : GameplayStaticsManager
{
    /*GameManager which should manage the Session across scenes, hold references to gameobjects within the scene, host GameModes and UI (plus interaction between GameMode and UI)*/

    [Header("Scene Settings")]
    public GameModeType gameMode = GameModeType.Lesson;
    public float MultiplyMagnitudeWith = 1f;
    public bool ObstacleCreationAtStart = false;

    [Header("Scene Objects")]
    public Camera arCamera;
    public GameObject playerGameObject, targetGameObject, obstaclePrefab;
    public Text timerText, launchText;
    public TMP_Text actionNeededText;
    public GameObject levelMissionCanvas, levelActiveCanvas, levelEndCanvas;

    [Header("DEVELOPMENT Only")]
    public Text DebugText;
    public GamePhase gamePhase = GamePhase.SelectPlane;

    public GameMode activeMode;

    void Awake()
    {
        actionNeededText.text = "move device slowly until indicator appears";
        SetGameMode();
        
    }

    public void SetGameMode() 
    {
        activeMode = new ModeLesson(this);
        activeMode.FreezeTime();
    }

    void Update()
    {
        UpdateTime();
        UpdatePlayerTouchInput();
        UpdateGameMode();
    }

    void FixedUpdate()
    {
        TouchInput.SetUIAreaToDeviceOrientation(Input.deviceOrientation);
        if (Input.GetKey(KeyCode.Escape))
            ResetButton();
    }

    void UpdateLessonUICanvas() 
    {
        timerText.text ="GameTime: " + (activeMode.bTimeLimit-Mathf.Round(GameTime.gameTime));
        actionNeededText.text = activeMode.actionNeededText;
        if (activeMode.levelEnd)
        {
            levelActiveCanvas.SetActive(false);
            levelEndCanvas.SetActive(true);
        }
    }

    void UpdateFreeRoamUICanvas()
    {
        actionNeededText.text = activeMode.actionNeededText;
    }

    void UpdateGameMode() 
    {
        //mode specifics
        switch (gameMode)
        {
            case GameModeType.Lesson:
            {
                UpdateLessonUICanvas();
                break;
            }
            case GameModeType.FreeRoam:
            {   
                UpdateFreeRoamUICanvas();
                break;
            }
        }
        activeMode.UpdateGameMode();
    }

    public void LaunchButton() 
    {
        if (gameMode == GameModeType.Lesson)
        {
            launchText.text = "launched!";
            activeMode.bLaunched = true;
            activeMode.UnfreezeTime();
            playerGameObject.GetComponent<PAMovement>().LaunchPlayerPlanet();
        }
    }

    public void ResetButton() 
    {
        launchText.text = "LAUNCH!";
        levelActiveCanvas.SetActive(true);
        levelEndCanvas.SetActive(false);
        activeMode.Reset();
    }
}
