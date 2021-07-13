using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheBigBanger.Formulae;

public enum EPAMovementTypes 
{
    Swipe,
    Rocket,
    Everything,
    None
}

public class PAMovement : PlanetMovementBase
{
    [Header("Player Configuration")]
    public EPlayerAbilities planetVelocityBy = EPlayerAbilities.swipeMovement;
    public EFactorElement PlayerInputFactor = EFactorElement.V;
    public float rotationSensitivity = 1f;
    [Tooltip(FormulaSheets.tooltip)]
    public string ForceIs = FormulaSheets.ForceIs[0];


    [Header("Prefab Objects")]
    public LineRenderer lineRenderer;
    public AnimationCurve lineWidthCurve;
    public float lineWidthMultiplier = 2f;

    public float timer = 0;

    private void Update()
    {
        if (bIsMoving)
        {
            UpdateMovePlanet();
            timer += Time.deltaTime;
        }
    }

    protected override void UpdateMovePlanet()
    {
        float tempMovement = GetVelocityFromAbility(planetVelocityBy);
        currentMovement = manager.activeMode.aSwipeMovement.swipeDirection* tempMovement;
        base.UpdateMovePlanet();
    }

    public float GetForceFromAbility(EPlayerAbilities ability)
    {
        float abilityVelocity = GetVelocityFromAbility(ability);

        if (ForceIs == FormulaSheets.ForceIs[0]/*F=1/2mv²*/ && PlayerInputFactor == EFactorElement.V)
            force = 0.5f * mass * Mathf.Pow(abilityVelocity, 2);
        else if (ForceIs == FormulaSheets.ForceIs[1]/*F=ma*/ && PlayerInputFactor == EFactorElement.A)
            force = mass * abilityVelocity;

        return force;
    }


    public float GetVelocityFromAbility(EPlayerAbilities ability)
    {
        velocity = GetMagnitudeFromAbility(ability);
        return velocity;
    }

    float GetMagnitudeFromAbility(EPlayerAbilities ability) 
    {
        float abilityMagnitude = 1f;
        switch (ability)
        {
            case EPlayerAbilities.swipeMovement:
                ForceIs = FormulaSheets.ForceIs[0];
                abilityMagnitude = manager.activeMode.aSwipeMovement.swipeMagnitude;
                break;
            case EPlayerAbilities.rocketMovement:
                ForceIs = FormulaSheets.ForceIs[1];
                abilityMagnitude = manager.activeMode.aRocketControl.rocketMagnitude * timer; ;
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
        if (manager.activeMode.aSwipeMovement.initialized)
        {
            manager.DebugText.text = "Launched with " + GetForceFromAbility(EPlayerAbilities.swipeMovement).ToString();
            bIsMoving = true;
            manager.activeMode.bLaunched = true;
        }
    }

    public override void DestroyPlanet()
    {
        base.DestroyPlanet();
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public override void ResetPlanet() 
    {
        base.ResetPlanet();
        //rotation
        transform.GetChild(0).gameObject.SetActive(true);
        timer = 0;
    }
}
