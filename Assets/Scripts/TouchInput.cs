using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/*Library of functions which are static and should make input on mobile device touch screens more accesbile.*/
public class TouchInput : MonoBehaviour
{
    public static ARRaycastManager arRaycastManager = new ARRaycastManager();
    public static Vector2 reservedUIArea, reservedHorizontalUIArea = new Vector2(0.85f, 0.35f), reservedVerticalUIArea = new Vector2(0.5f, 0.1f);
    static RaycastHit[] raycastHits;

    public static void ResetInput() 
    {
        raycastHits = new RaycastHit[] { };
        reservedUIArea = Vector2.zero;
        arRaycastManager = new ARRaycastManager();
    }

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
        raycastHits = Physics.RaycastAll(cam.ScreenPointToRay(GetTouchPosition()));
        return raycastHits.Length > 0;
    }

    public static Vector3 GetHitWorldPositionAtLayer(int targetLayer)
    {
        foreach(RaycastHit hit in raycastHits) 
        {
            if (hit.collider.gameObject.layer == targetLayer)
                return hit.point;
        } return default;
    }

    public static bool IsPlayerHit()
    {
        //player layer currently at 6
        return IsTargetLayerHit(6);
    }

    public static bool IsInputCanvasHit()
    {
        //player layer currently at 3
        return IsTargetLayerHit(3);
    }

    public static bool IsRotationSocketHit()
    {
        //player layer currently at 8
        return IsTargetLayerHit(8);
    }

    static bool IsTargetLayerHit(int targetLayer) 
    {
        foreach (RaycastHit hit in raycastHits)
        {
            if (hit.collider.gameObject.layer == targetLayer)
                return true;
        } return false;
    }

    public static void TitleScreenIfBackButton() 
    {
        //if (Application.platform == RuntimePlatform.Android) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    public static bool IsUIHit() 
    {
        return GetRelativeViewportTouchPosition().x > reservedUIArea.x && GetRelativeViewportTouchPosition().y < reservedUIArea.y;
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

