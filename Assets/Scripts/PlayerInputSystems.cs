using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace TheBigBanger.PlayerInputSystems
{
    /*Library of functions which are static and should make input on mobile device touch screens more accesbile.*/
    public class TouchInput : MonoBehaviour
    {
        static Vector2 touchPosition;
        public static ARRaycastManager arRaycastManager = new ARRaycastManager();
        static List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();
        static RaycastHit raycastHit;

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
            touchPosition = GetTouchPosition();
        }

        public static bool RaycastAgainstTrackable(TrackableType trackableType)
        {
            return arRaycastManager.Raycast(touchPosition, raycastHits, trackableType);
        }

        public static ARRaycastHit GetARHit()
        {
            return raycastHits[0];
        }

        public static Pose GetARHitPose() 
        {
            return raycastHits[0].pose;
        }

        public static bool RaycastFromCamera(Camera cam) 
        {
            return Physics.Raycast(cam.ScreenPointToRay(GetTouchPosition()), out raycastHit);
        }

        public static RaycastHit GetHit()
        {
            return raycastHit;
        }

        public static GameObject GetHitObject()
        {
            return GetHit().collider.gameObject;
        }

        public static bool IsPlayerHit()
        {
            //player layer current at 6
            return GetHitObject().layer == 6;
        }
    }
}
