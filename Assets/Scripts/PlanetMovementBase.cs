using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class PlanetMovementBase : MonoBehaviour
{
    [Header("Level Configuration")]
    public float mass = 1;
    public GameManager manager;

    [Header("Debug")]
    public Vector3 _currentForce, _currentSpeed;
    public bool bIsMoving = false;

    void Start()
    {
        manager = GameObject.Find("/GameManager").GetComponent<GameManager>();
    }

    /*protected virtual void UpdateMovePlanet() 
    {

    }

    Vector3 GetPlayerInputFactor() 
    {
        return manager.playerGameObject.
    }*/
}
