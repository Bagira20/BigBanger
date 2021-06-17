using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace TheBigBanger.PlayerInputSystems
{
    public class TouchInput : MonoBehaviour
    {
        public static Vector2 touchPosition;
        public static ARRaycastManager arRaycastManager;
        static List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();

        public static bool IsTouching() 
        {
            return Input.touchCount > 0;
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

        public static bool DoRaycastAgainstTrackable(TrackableType trackableType)
        {
            return arRaycastManager.Raycast(touchPosition, raycastHits, trackableType);
        }

        public static Pose GetHitPose() 
        {
            return raycastHits[0].pose;
        }
    }
}
