using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using TheBigBanger.GameplayStatics;
using TheBigBanger.Formulae;

[System.Serializable]
public struct MovementConfigSet
{
    public EFactorElement configuredFactor;
    public float value;
}

public class PlanetMovementBase : MonoBehaviour
{
    public GameManager manager;
    public float mass = 1f, velocity, acceleration, force;
    protected CanvasManager canvas;

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
        canvas = manager.canvas;
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
                case EFactorElement.M:
                    mass = moveSet.value; break;
                case EFactorElement.A:
                    acceleration = moveSet.value; break;
                case EFactorElement.V:
                    velocity = moveSet.value; break;
                case EFactorElement.F:
                    force = moveSet.value; break;
            }
        }
    }

    protected bool IsPreConfigured(EFactorElement factor) 
    {
        foreach (MovementConfigSet moveSet in MovementConfiguration)
        {
            if (moveSet.configuredFactor == factor)
                return true;
        } return false;
    }

    public virtual void LaunchPlanet() 
    {
        bIsMoving = true;
    }

    public virtual void DestroyPlanet() 
    {
        bIsMoving = false;
        GetComponent<MeshRenderer>().enabled = false;
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
    }

    public virtual void ResetPlanet()
    {
        transform.position = startPos;
        bIsMoving = false;
        GetComponent<MeshRenderer>().enabled = true;
        foreach (Transform child in transform)
            child.gameObject.SetActive(true);
    }
}