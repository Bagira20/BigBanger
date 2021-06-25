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
    public InputFactor PlayerInputControlsFactor = InputFactor.V;
    public float MultiplyMagnitudeWith = 1f;

    [Header("Scene Objects")]
    public GameObject playerGameObject;
    public Camera arCamera;
    public Text timerText;
    //public TMP_Text actionNeededText; 

    [Header("DEVELOPMENT Only")]
    public Text DebugText;
    public GamePhase gamePhase = GamePhase.LevelStart; /*PLS CHANGE LATEEEEERRRRRRRRRRRRRRRR!!!!!!!!!!!*/

    public GameMode _activeMode;

    void Awake()
    {
        SetGameMode();
        
    }

    public void SetGameMode() 
    {
        _activeMode = new ModeLesson(this);
        _activeMode.UnfreezeTime();  /*PLS CHANGE LATEEEEERRRRRRRRRRRRRRRR!!!!!!!!!!!*/
    }

    void Update()
    {
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                DebugText.text = raycastHit.collider.name;
            }
        }

        UpdateTime();
        UpdatePlayerTouchInput();
        UpdateGameMode();

    }

    void UpdateLessonUICanvas() 
    {
        timerText.text ="GameTime: " + (_activeMode.bTimeLimit-Mathf.Round(GameTime.gameTime));
        //actionNeededText.text = _activeMode.actionNeededText;
    }

    void UpdateFreeRoamUICanvas()
    {
        //actionNeededText.text = _activeMode.actionNeededText;
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
                UpdateFreeRoam();
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
                if (!_activeMode.bLaunched && !GameTime.bFreeze && TouchInput.IsPlayerHit())
                   _activeMode.FreezeTime();

                _activeMode.Feedback();
            }
        }
    }

    void UpdateFreeRoam() 
    {

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
}
