using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheBigBanger.Formulae;

public class PAMovement : PlanetMovementBase
{
    public LineRenderer lineRenderer;

    /*protected override void UpdateMovePlanet()
    {
        base.UpdateMovePlanet();
        //gh
    }*/

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
                abilityMagnitude = manager._activeLesson.aSwipeMovement.swipeMagnitude;
                break;
                /*case PlayerAbilityList.playerAbilities.rocketMovement:
                    abilityMagnitude = manager._activeLesson.aRocketControl.;
                    break;*/
        }

        return abilityMagnitude * manager.MultiplyMagnitudeWith;
    }
}
