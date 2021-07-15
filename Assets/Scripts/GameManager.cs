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
    public EGameModeType gameModeType = EGameModeType.Lesson;
    public float MultiplyMagnitudeWith = 1f;
    public bool ObstacleCreationAtStart = false;

    [Header("Scene Objects")]
    public Camera arCamera;
    public GameObject playerGameObject, targetGameObject, obstaclePrefab;
    public Text timerText, launchText;
    public TMP_Text actionNeededText, score;
    public GameObject levelMissionCanvas, levelActiveCanvas, levelEndCanvas;

    [Header("DEVELOPMENT Only")]
    public Text DebugText;
    public EGamePhase gamePhase = EGamePhase.SelectPlane;
    public int levelIntroNr = 0;

    public GameMode activeMode;
    public CanvasManager canvas;

    void Awake()
    {
        ResetStatics();
        actionNeededText.text = "move device slowly until indicator appears";
        SetGameMode();
        SetUI();
    }

    public void SetGameMode()
    {
        activeMode = new ModeLesson(this);
        activeMode.FreezeTime();
    }

    void SetUI() 
    {
        if (canvas == null)
            canvas = GameObject.Find("UICanvas").GetComponent<CanvasManager>();
    }

    void Update()
    {
        UpdateTime();
        UpdatePlayerTouchInput();
        UpdateGameMode();
        if (activeMode.IsTimeOver())
        {
            activeMode.gamePhase = EGamePhase.LevelEnd;
            activeMode.UpdateGameMode();
            activeMode.gamepass = 0;
        }
    }

    void FixedUpdate()
    {
        TouchInput.SetUIAreaToDeviceOrientation(Input.deviceOrientation);
        if (Input.GetKey(KeyCode.Escape))
            ResetButton();
    }

    void UpdateLessonUICanvas()
    {
        canvas.SetTimeCounter(activeMode.bTimeLimit - Mathf.Round(GameTime.gameTime));
        actionNeededText.text = activeMode.actionNeededText;
        activeMode.aSwipeMovement.UpdateSwipeUI();
        if (activeMode.levelEnd)
        {
            CalculateScore();
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
        switch (gameModeType)
        {
            case EGameModeType.Lesson:
                {
                    UpdateLessonUICanvas();
                    break;
                }
            case EGameModeType.FreeRoam:
                {
                    UpdateFreeRoamUICanvas();
                    break;
                }
        }
        activeMode.UpdateGameMode();
    }

    public void LaunchButton()
    {
        if (gameModeType == EGameModeType.Lesson)
        {
            launchText.text = "launched!";
            activeMode.bLaunched = true;
            activeMode.UnfreezeTime();
            playerGameObject.GetComponent<PAMovement>().LaunchPlanet();
            targetGameObject.GetComponent<PBMovement>().LaunchPlanet();
        }
    }

    public void ResetButton()
    {
        launchText.text = "LAUNCH!";
        levelActiveCanvas.SetActive(true);
        levelEndCanvas.SetActive(false);
        activeMode.Reset();
    }

    public void RocketButton()
    { 
        if (activeMode.aRocketControl.rocketCount <5 && !activeMode.bLaunched)
        {
            activeMode.aRocketControl.rocketCount++;
            activeMode.aRocketControl.UpdateRocketMagnitude();

            DebugText.text = activeMode.aRocketControl.rocketCount.ToString() + " -- " + activeMode.aRocketControl.rocketMagnitude.ToString();
        }
    }    

    public string CalculateScore()
    {
        string score;
        float paForce, pbForce;
        paForce = playerGameObject.GetComponent<PAMovement>().GetForce();
        pbForce = targetGameObject.GetComponent<PBMovement>().GetForce();
        
        score = ((1000f - (paForce - pbForce) + GameTime.gameTime*3)*activeMode.gamepass).ToString();
        this.score.text = score;
        return score;
    }
}
