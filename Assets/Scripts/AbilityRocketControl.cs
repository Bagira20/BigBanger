using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityRocketControl : AbilityBase
{
    public float rocketAcceleration = 0.2f;
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

        foreach (Transform child in rocketsGameObject)
        {
            child.gameObject.SetActive(false);
        }    

    }

    public void UpdateRocketMagnitude()
    {
        rocketMagnitude = rocketAcceleration * rocketCount;
        for (int i=0; i<rocketCount; i++)
        {
            rocketsGameObject.GetChild(i).gameObject.SetActive(true);
        }
    }

}
