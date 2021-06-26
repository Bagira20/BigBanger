using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheBigBanger.Formulae;

public class PAMovement : PlanetMovementBase
{
    [Header("Player Configuration")]
    public FactorElement PlayerInputFactor = FactorElement.V;

    [Header("Prefab Objects")]
    public LineRenderer lineRenderer;

    private void Update()
    {
        if (bIsMoving)
            UpdateMovePlanet();
    }

    protected override void UpdateMovePlanet()
    {
        float tempMovement = GetVelocityFromAbility(PlayerAbilityList.playerAbilities.swipeMovement);
        _currentMovement = manager._activeMode.aSwipeMovement.swipeDirection* tempMovement;
        base.UpdateMovePlanet();
    }

    public float GetForceFromAbility(PlayerAbilityList.playerAbilities ability)
    {
        float abilityVelocity = GetVelocityFromAbility(ability);

        if (manager.ForceEqualTo == FormulaSheets.ForceIs[0]/*F=1/2mv²*/ && PlayerInputFactor == FactorElement.V)
            force = 0.5f * mass * Mathf.Pow(abilityVelocity, 2);
        else if (manager.ForceEqualTo == FormulaSheets.ForceIs[1]/*F=ma*/ && PlayerInputFactor == FactorElement.A)
            force = mass * abilityVelocity;

        return force;
    }


    public float GetVelocityFromAbility(PlayerAbilityList.playerAbilities ability)
    {
        velocity = GetMagnitudeFromAbility(ability);
        return velocity;
    }

    float GetMagnitudeFromAbility(PlayerAbilityList.playerAbilities ability) 
    {
        float abilityMagnitude = 1f;
        switch (ability)
        {
            case PlayerAbilityList.playerAbilities.swipeMovement:
                abilityMagnitude = manager._activeMode.aSwipeMovement.swipeMagnitude;
                break;
            case PlayerAbilityList.playerAbilities.rocketMovement:
                abilityMagnitude = manager._activeMode.aRocketControl.rocketMagnitude;
                break;
        }

        return abilityMagnitude * manager.MultiplyMagnitudeWith;
    }

    public float GetForce() 
    {
        return force;
    }

    public float GetMass()
    {
        return mass;
    }

    public float GetVelocity() 
    {
        return velocity;
    }


    public void LaunchPlayerPlanet()
    {
        if (manager._activeMode.aSwipeMovement.initialized)
        {
            manager.DebugText.text = "Launched with " + GetForceFromAbility(PlayerAbilityList.playerAbilities.swipeMovement).ToString();
            bIsMoving = true;
            manager._activeMode.bLaunched = true;
        }
    }
}
