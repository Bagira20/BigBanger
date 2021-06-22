using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/*Library of functions which are static and should make input on mobile device touch screens more accesbile.*/
public class TouchInput : MonoBehaviour
{
    static Vector2 _touchPosition;
    public static ARRaycastManager arRaycastManager = new ARRaycastManager();
    static List<ARRaycastHit> _raycastHits = new List<ARRaycastHit>();
    static RaycastHit _raycastHit;

    public static bool IsTouching()
    {
        return Input.touchCount > 0;
    }

    public static Touch GetTouch()
    {
        return Input.GetTouch(0);
    }

    public static Vector2 GetTouchPosition()
    {
        if (IsTouching())
            return Input.GetTouch(0).position;
        return default;
    }

    public static void SetTouchPosition()
    {
        _touchPosition = GetTouchPosition();
    }

    public static bool RaycastAgainstTrackable(TrackableType trackableType)
    {
        return arRaycastManager.Raycast(_touchPosition, _raycastHits, trackableType);
    }

    public static ARRaycastHit GetARHit()
    {
        //RaycastAgainstTrackable needs to be called first
        return _raycastHits[0];
    }


    public static bool RaycastFromCamera(Camera cam)
    {
        return Physics.Raycast(cam.ScreenPointToRay(GetTouchPosition()), out _raycastHit);
    }

    public static RaycastHit GetHit()
    {
        //RaycastFromCamera needs to be called first
        return _raycastHit;
    }

    public static Vector3 GetHitWorldPosition()
    {
        return GetHit().point;
    }

    public static GameObject GetHitObject()
    {
        return GetHit().collider.gameObject;
    }

    public static bool IsPlayerHit()
    {
        //player layer currently at 6
        return GetHitObject().layer == 6;
    }

    public static bool IsInputCanvasHit()
    {
        //player layer currently at 3
        return GetHitObject().layer == 3;
    }
}

