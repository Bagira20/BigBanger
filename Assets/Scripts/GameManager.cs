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

    [Header("Scene Objects")]
    public Camera arCamera;
    public GameObject playerGameObject, targetGameObject;
    public Text timerText;
    public TMP_Text actionNeededText; 

    [Header("DEVELOPMENT Only")]
    public Text DebugText;
    public GamePhase gamePhase = GamePhase.SelectPlane; /*PLS CHANGE LATEEEEERRRRRRRRRRRRRRRR!!!!!!!!!!!*/

    public GameMode activeMode;

    void Awake()
    {
        actionNeededText.text = "move device slowly until indicator appears";
        SetGameMode();
        
    }

    public void SetGameMode() 
    {
        activeMode = new ModeLesson(this);
        activeMode.FreezeTime();  /*PLS CHANGE LATEEEEERRRRRRRRRRRRRRRR!!!!!!!!!!!*/
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
    }

    void UpdateFreeRoamUICanvas()
    {
        actionNeededText.text = activeMode.actionNeededText;
    }

    void UpdateGameMode() 
    {
        activeMode.Feedback();

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

    }

    public void LaunchButton() 
    {
        if (gameMode == GameModeType.Lesson)
        {
            GameObject.Find("/UICanvas/LaunchButton/Text").GetComponent<Text>().text = "launched!";
            activeMode.bLaunched = true;
            activeMode.UnfreezeTime();
            playerGameObject.GetComponent<PAMovement>().LaunchPlayerPlanet();
        }
    }

    public void ResetButton() 
    {
        GameObject.Find("/UICanvas/LaunchButton/Text").GetComponent<Text>().text = "LAUNCH!";
        activeMode.Reset();
    }
}
