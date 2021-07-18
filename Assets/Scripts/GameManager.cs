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
    public GameObject playerGameObject, targetGameObject;
    public GameObject[] obstacles;
    public Text timerText, launchText;
    public Text actionNeededText, score, playerScoreStats, playerTargetStats;
    public GameObject levelMissionCanvas, timeOverCanvas, actionText;
    public GameObject[] playPhaseCanvasObjects, scoreCanvas;

    [Header("DEVELOPMENT Only")]
    public Text DebugText;
    public EGamePhase gamePhase = EGamePhase.SelectPlane;
    public int levelIntroNr = 0;

    public GameMode activeMode;
    [HideInInspector]
    public CanvasManager canvas;
    [Header("Rocket Mode")]
    public ParticleSystem rocketLaunchCore;
    public List<GameObject> rocketLaunchSides;

    void Awake()
    {
        ResetStatics();
        actionNeededText.text = "move device slowly until indicator appears";
        SetGameMode();
        SetUI();
        playerGameObject.SetActive(true);
        rocketLaunchCore = playerGameObject.transform.Find("RocketFlames").gameObject.GetComponent<ParticleSystem>();
        rocketLaunchSides = new List<GameObject>();
        foreach (Transform child in rocketLaunchCore.gameObject.transform)
        {
            if (child.name.Contains("Flames "))
            {
                rocketLaunchSides.Add(child.gameObject);
                child.gameObject.SetActive(false);
            }
        }
        playerGameObject.SetActive(false);
    }

    public void SetGameMode()
    {
        activeMode = new ModeLesson(this);
        activeMode.FreezeTime();
    }

    void SetUI() 
    {
        SetPlayPhaseUI(false);
        if (activeMode.playerMovement.planetVelocityBy == EPlayerAbilities.rocketMovement)
            canvas.SetRocketCountUI(0, 1f);
        if (canvas == null)
            canvas = GameObject.Find("UICanvas").GetComponent<CanvasManager>();
    }

    void Update()
    {
        DebugText.text = TouchInput.GetRelativeViewportTouchPosition().ToString();
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
            if (activeMode.IsTimeOver())
                timeOverCanvas.SetActive(true);
            else
            {
                CalculateScore();
                SetPlayPhaseUI(false);
                SetScoreUI(true);
            }
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

    public void SetPlayPhaseUI(bool bActive) 
    {
        foreach (GameObject playPhaseUIObject in playPhaseCanvasObjects)
            playPhaseUIObject.SetActive(bActive);
    }
    public void SetScoreUI(bool bActive)
    {
        foreach (GameObject scoreUIObject in scoreCanvas)
            scoreUIObject.SetActive(bActive);
    }

    public void LaunchResetButton() 
    {
        if (activeMode.bLaunched)
        {
            ResetButton();
            launchText.text = "LAUNCH";
        }
        else
        {
            LaunchButton();
            launchText.text = "RESET";
        }
    }

    public void LaunchButton()
    {
        if (gameModeType == EGameModeType.Lesson)
        {
            launchText.text = "launched!";
            activeMode.bLaunched = true;
            activeMode.UnfreezeTime();
            activeMode.aSwipeMovement.rotationSocket.SetActive(false);
            canvas.PlayerMassText.CanvasElement.SetActive(false);
            canvas.LineText.CanvasElement.SetActive(false);
            if (playerGameObject.GetComponent<PAMovement>().planetVelocityBy == EPlayerAbilities.rocketMovement)
            {
                StartCoroutine(StartPlanetMovement(1f));
                for (int i=0; i< activeMode.aRocketControl.rocketCount; i++)
                {
                    rocketLaunchSides[i].SetActive(true);
                    rocketLaunchCore.Play(true);
                    StartCoroutine(StartRocketSounds());
                }
                canvas.SetRocketCountUI(Mathf.FloorToInt(activeMode.aRocketControl.rocketCount), 0.6f);
            }
            else
                StartCoroutine(StartPlanetMovement(0.05f));
        }
    }

    IEnumerator StartRocketSounds() 
    {
        yield return new WaitForSeconds(0.75f);
        AudioPlayer.Play3DAudioFromRange(activeMode.playerMovement.audioSource, activeMode.playerMovement.RocketBoostSounds, new Vector2(0.85f, 1.2f), new Vector2(0.85f, 1.05f));
    }

    IEnumerator StartPlanetMovement(float waitingTime) 
    {
        playerGameObject.GetComponent<PAMovement>().PlayLaunchSound();
        yield return new WaitForSeconds(waitingTime);
        playerGameObject.GetComponent<PAMovement>().LaunchPlanet();
        targetGameObject.GetComponent<PBMovement>().LaunchPlanet();
    }

    public void ResetButton()
    {
        launchText.text = "LAUNCH!";
        SetPlayPhaseUI(true);
        SetScoreUI(false);
        timeOverCanvas.SetActive(false);
        activeMode.Reset();
        activeMode.aSwipeMovement.rotationSocket.SetActive(true);
        rocketLaunchCore.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        AudioPlayer.Play2DAudioFromRange(activeMode.playerMovement.audioSource, canvas.CancelSounds, new Vector2(0.8f, 1.2f), new Vector2(0.95f, 1.1f));
        canvas.SetRocketCountUI(Mathf.FloorToInt(activeMode.aRocketControl.rocketCount), 1f);
        activeMode.UnfreezeTime();
    }

    public void RocketButton()
    {
        if (!activeMode.bLaunched)
        {
            if (activeMode.aRocketControl.rocketCount < 5)
                activeMode.aRocketControl.rocketCount++;
            else
                activeMode.aRocketControl.rocketCount = 1;
            activeMode.aRocketControl.UpdateRocketMagnitude();
            canvas.SetRocketCountUI(Mathf.FloorToInt(activeMode.aRocketControl.rocketCount), 1f);
            AudioPlayer.Play2DAudioFromRange(activeMode.playerMovement.audioSource, canvas.attachRocketSounds, new Vector2(0.95f, 1.05f), new Vector2(0.95f, 1.1f));
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


    //time constraints, code below is not beautiful, we know
    public string GetPlayerStatsString() 
    {
        float mass, velocity, acceleration, force;
        string returnString = "You've Completed LEVEL " + (levelIntroNr + 1) + "!\n\n";

        mass = GetTransformedValue(activeMode.playerMovement.GetMass());
        if (activeMode.playerMovement.planetVelocityBy == EPlayerAbilities.swipeMovement)
        {
            velocity = GetTransformedValue(activeMode.playerMovement.GetVelocityFromAbility(activeMode.playerMovement.planetVelocityBy));
            force = 0.5f * mass * Mathf.Pow(velocity, 2);
            acceleration = 0f;
            return "FORCE: " + force + " N\nVELOCITY: " + velocity + "m/s\nMASS: " + mass + "kg\n\nPLAYER PLANET"; 
        }
        else if (activeMode.playerMovement.planetVelocityBy == EPlayerAbilities.rocketMovement) 
        {
            acceleration = GetTransformedValue(activeMode.aRocketControl.rocketMagnitude);
            force = mass * acceleration;
            velocity = 0f;
            return "FORCE: " + force + " N\nACCELERATION: " + acceleration + "m/s²\nMASS: " + mass + "kg\n\nPLAYER PLANET";
        }

        return returnString;
    }

    public string GetTargetStatsString()
    {
        float mass, velocity, force;

        mass = GetTransformedValue(activeMode.targetMovement.mass);

        velocity = GetTransformedValue(activeMode.targetMovement.velocity);
        force = 0.5f * mass * Mathf.Pow(velocity, 2);
        return "FORCE: " + force + " N\nVELOCITY: " + velocity + "m/s\nMASS: " + mass + "kg\n\nTARGET PLANET";
    }

}
