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
    public int value;
}

public class PlanetMovementBase : MonoBehaviour
{
    public GameManager manager;
    protected float mass = 1f, velocity, acceleration, force;

    [Header("Level Configuration")]
    public PlayerAbilityList.playerAbilities planetVelocityBy = PlayerAbilityList.playerAbilities.swipeMovement;
    [Tooltip("Defined values which represent fixed context of current scene")]
    public List<MovementConfigSet> MovementConfiguration = new List<MovementConfigSet>();
    [Tooltip(FormulaSheets.tooltip)]
    public string ForceIs = FormulaSheets.ForceIs[0];

    [HideInInspector]
    public Vector3 _currentForce;
    [HideInInspector]
    public bool bIsMoving = false;

    void Start()
    {
        manager = GameObject.Find("/GameManager").GetComponent<GameManager>();
        ConfigurateMovement();
    }

    protected virtual void UpdateMovePlanet() 
    {
        transform.position += _currentForce * GameTime.deltaTime;
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