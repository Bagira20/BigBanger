using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheBigBanger.Formulae;

public class PAMovement : PlanetMovementBase
{
    [Header("Prefab Objects")]
    public LineRenderer lineRenderer;

    private void Update()
    {
        if (bIsMoving && manager._activeMode.bLaunched)
            base.UpdateMovePlanet();
    }

    protected override void UpdateMovePlanet()
    {
        float tempForce = GetForceFromAbility(PlayerAbilityList.playerAbilities.swipeMovement);
        _currentForce = manager._activeMode.aSwipeMovement.swipeDirection*tempForce;
        base.UpdateMovePlanet();
    }

    public float GetForceFromAbility(PlayerAbilityList.playerAbilities ability)
    {
        float abilityMagnitude = GetMagnitudeFromAbility(ability);

        if (manager.ForceEqualTo == FormulaSheets.ForceIs[0]/*F=ma*/ && manager.PlayerInputControlsFactor == InputFactor.A)
            return mass * abilityMagnitude;
        else if (manager.ForceEqualTo == FormulaSheets.ForceIs[1]/*F=1/2mv²*/ && manager.PlayerInputControlsFactor == InputFactor.V)
            return 0.5f * mass * Mathf.Pow(abilityMagnitude, 2);

        return 1f;
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

    public void LaunchPlayerPlanet()
    {
        if (manager._activeMode.aSwipeMovement.initialized)
        {
            manager.DebugText.text = "Launched with " + GetForceFromAbility(PlayerAbilityList.playerAbilities.swipeMovement).ToString();
            bIsMoving = true;
        }
    }
}
