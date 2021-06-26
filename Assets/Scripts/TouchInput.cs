using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/*Library of functions which are static and should make input on mobile device touch screens more accesbile.*/
public class TouchInput : MonoBehaviour
{
    public static ARRaycastManager arRaycastManager = new ARRaycastManager();
    public static Vector2 reservedUIArea, reservedHorizontalUIArea = new Vector2(0.3f, 0.3f), reservedVerticalUIArea = new Vector2(0.6f, 0.1f);
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

    public static Vector2 GetRelativeViewportTouchPosition() 
    { 
        return Camera.main.ScreenToViewportPoint(GetTouchPosition()); 
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

    public static bool IsUIHit() 
    {
        return GetRelativeViewportTouchPosition().x < reservedUIArea.x && GetRelativeViewportTouchPosition().y < reservedUIArea.y;
    }

    public static void SetUIAreaToDeviceOrientation(DeviceOrientation deviceOrientation) 
    {
        switch (deviceOrientation) 
        {
            case DeviceOrientation.LandscapeLeft:
            case DeviceOrientation.LandscapeRight:
                reservedUIArea = reservedHorizontalUIArea;
                break;
            case DeviceOrientation.Portrait:
            case DeviceOrientation.PortraitUpsideDown:
                reservedUIArea = reservedVerticalUIArea;
                break;
        }
    }
}

