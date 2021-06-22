using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

public class ARLibrary : MonoBehaviour
{
    static GameObject placementIndicator;
    static ARRaycastManager arRaycastManager;
    static Pose placementPose;
    static bool placementPoseIsValid = false;
    

    static public void Initialize()
    {
        placementIndicator = GameObject.Find("PlacementIndicator");
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
    }
    // Update is called once per frame
    static public void UpdateARLibrary()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
    }

    static void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    static void UpdatePlacementPose()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        arRaycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon);
        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;

            //to rotate indicator to camera
            Vector3 cameraForward = Camera.current.transform.forward;
            Vector3 cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z);
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

}
