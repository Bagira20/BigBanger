using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

public class ARLibrary : MonoBehaviour
{
    public GameObject placementIndicator;
    private ARSessionOrigin _arOrigin;
    private ARRaycastManager _arRaycastManager;
    private Pose _placementPose;
    private bool placementPoseIsValid = false;

    private void Start()
    {
        _arOrigin = FindObjectOfType<ARSessionOrigin>();
        _arRaycastManager = FindObjectOfType<ARRaycastManager>();
    }
    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(_placementPose.position, _placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        _arRaycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon);
        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            _placementPose = hits[0].pose;

            //to rotate indicator to camera
            Vector3 cameraForward = Camera.current.transform.forward;
            Vector3 cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z);
            _placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    public Pose GetIndicatorPose() 
    {
        return _placementPose;
    }
}
