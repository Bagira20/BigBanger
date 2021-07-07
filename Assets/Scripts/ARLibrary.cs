using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using TheBigBanger.GameModeManager;
using UnityEngine.Experimental.XR;
using UnityEngine.UI;

public class ARLibrary : MonoBehaviour
{
    public GameObject placementIndicator;
    public ARRaycastManager arRaycastManager;
    public ARSessionOrigin arSessionOrigin;
    public Pose placementPose;
    public bool placementPoseIsValid = false;
    public bool stopIndicator = false;
    public Text debug;

    GameManager gameManager;

    public void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        if (gameManager.activeMode.gamePhase == GamePhase.PlaceObstacles)
        {
            arSessionOrigin.GetComponent<ARPlaneManager>().planePrefab = null;
        }
        if (gameManager.activeMode.gamePhase == GamePhase.PlayPhase)
        {
            stopIndicator = true;
        }
    }

     void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid && !stopIndicator)
        {
            placementIndicator.SetActive(true);
            if (gameManager.activeMode.gamePhase == GamePhase.SelectPlane)
                gameManager.actionNeededText.text = "Please tap on a flat horizontal surface to initialize playground";
            else
                gameManager.actionNeededText.text = "";
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

     void UpdatePlacementPose()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        arRaycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon);
        placementPoseIsValid = hits.Count > 0;

        if (placementPoseIsValid)
        {

            placementPose = hits[0].pose;

            //to rotate indicator to camera
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z);
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

}
