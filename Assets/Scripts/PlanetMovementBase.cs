using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using TheBigBanger.GameplayStatics;
using TheBigBanger.Formulae;

[System.Serializable]
public struct MovementConfigSet
{
    public FactorElement configuredFactor;
    public float value;
}

public class PlanetMovementBase : MonoBehaviour
{
    public GameManager manager;
    protected float mass = 1f, velocity, acceleration, force;

    [Header("Level Configuration")]
    [Tooltip("Defined values which represent fixed context of current scene")]
    public List<MovementConfigSet> MovementConfiguration = new List<MovementConfigSet>();

    [HideInInspector]
    public Vector3 currentMovement, startPos;
    [HideInInspector]
    public bool bIsMoving = false;

    void Start()
    {
        manager = GameObject.Find("/GameManager").GetComponent<GameManager>();
        ConfigurateMovement();
    }

    protected virtual void UpdateMovePlanet() 
    {
        transform.position += currentMovement * GameTime.deltaTime;
    }

    void ConfigurateMovement() 
    {
        foreach(MovementConfigSet moveSet in MovementConfiguration) 
        {
            switch (moveSet.configuredFactor) 
            {
                case FactorElement.M:
                    mass = moveSet.value; break;
                case FactorElement.A:
                    acceleration = moveSet.value; break;
                case FactorElement.V:
                    velocity = moveSet.value; break;
                case FactorElement.F:
                    force = moveSet.value; break;
            }
        }
    }

    protected bool IsPreConfigured(FactorElement factor) 
    {
        foreach (MovementConfigSet moveSet in MovementConfiguration)
        {
            if (moveSet.configuredFactor == factor)
                return true;
        } return false;
    }
}