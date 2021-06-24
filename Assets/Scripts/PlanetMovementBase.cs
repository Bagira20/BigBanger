using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using TheBigBanger.GameplayStatics;

public class PlanetMovementBase : MonoBehaviour
{
    [Header("Level Configuration")]
    public float mass = 1;
    public GameManager manager;

    [Header("Debug")]
    public Vector3 _currentForce;
    public bool bIsMoving = false;

    void Start()
    {
        manager = GameObject.Find("/GameManager").GetComponent<GameManager>();
    }

    protected virtual void UpdatePlanetPosition() 
    {
        transform.position += _currentForce * GameTime.deltaTime;
    }
}
