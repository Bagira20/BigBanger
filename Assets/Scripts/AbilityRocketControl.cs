using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityRocketControl : AbilityBase
{
    public float rocketAcceleration = 0.1f;
    public float rocketMagnitude;
    public float rocketCount = 0;
    public GameObject playerGameObject;
    public Transform rocketsGameObject;

    public AbilityRocketControl(GameManager manager) : base(manager) 
    {

        rocketCount = 0;
        rocketMagnitude = 0;
        playerGameObject = manager.playerGameObject;
        rocketsGameObject = playerGameObject.transform.Find("Rockets");
    }

    public void ResetRocket()
    {
        rocketCount = 0;
        rocketMagnitude = 0;
        gameManager.canvas.SetRocketCountUI(0, 1f);

        foreach (Transform child in rocketsGameObject)
        {
            child.gameObject.SetActive(false);
        }    
    }

    public void UpdateRocketMagnitude()
    {
        //magnitude is the velocity
        rocketMagnitude = (rocketAcceleration * rocketCount);
        for (int i=0; i<5; i++)
        {
            if (i < rocketCount)
                rocketsGameObject.GetChild(i).gameObject.SetActive(true);
            else
                rocketsGameObject.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void UpdateRocketRotation(Vector3 swipeLineEndPosition) 
    {
        Vector3 oppositeTargetPosition = rocketsGameObject.transform.position - (swipeLineEndPosition - rocketsGameObject.transform.position);
        rocketsGameObject.transform.LookAt(oppositeTargetPosition);
        rocketsGameObject.transform.parent.LookAt(oppositeTargetPosition);
    }
}
