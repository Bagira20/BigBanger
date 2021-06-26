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
    [Tooltip(FormulaSheets.tooltip)]
    public string ForceEqualTo = FormulaSheets.ForceIs[1];
    public FactorElement PlayerInputControlsFactor = FactorElement.V;
    public float MultiplyMagnitudeWith = 1f;

    [Header("Scene Objects")]
    public GameObject playerGameObject, targetGameObject;
    public Camera arCamera;
    public Text timerText;
    public TMP_Text actionNeededText; 

    [Header("DEVELOPMENT Only")]
    public Text DebugText;
    public GamePhase gamePhase = GamePhase.SelectPlane; /*PLS CHANGE LATEEEEERRRRRRRRRRRRRRRR!!!!!!!!!!!*/

    public GameMode _activeMode;

    void Awake()
    {
        actionNeededText.text = "move device slowly until indicator appears";
        SetGameMode();
        
    }

    public void SetGameMode() 
    {
        _activeMode = new ModeLesson(this);
        _activeMode.FreezeTime();  /*PLS CHANGE LATEEEEERRRRRRRRRRRRRRRR!!!!!!!!!!!*/
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
    }

    void UpdateLessonUICanvas() 
    {
        timerText.text ="GameTime: " + (_activeMode.bTimeLimit-Mathf.Round(GameTime.gameTime));
        actionNeededText.text = _activeMode.actionNeededText;
    }

    void UpdateFreeRoamUICanvas()
    {
        actionNeededText.text = _activeMode.actionNeededText;
    }

    void UpdateGameMode() 
    {
        _activeMode.Feedback();

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
            _activeMode.bLaunched = true;
            _activeMode.UnfreezeTime();
            playerGameObject.GetComponent<PAMovement>().LaunchPlayerPlanet();
        }
    }

    public void ResetButton() 
    {
        GameObject.Find("/UICanvas/LaunchButton/Text").GetComponent<Text>().text = "LAUNCH!";
        _activeMode.Reset();
    }
}
