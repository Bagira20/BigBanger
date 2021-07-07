using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityList
{
    public enum playerAbilities { swipeMovement, rocketMovement, everything, none};

    /*non-static library for all abilities*/
    public AbilitySwipeMovement aSwipeMovement;
    public AbilityRocketControl aRocketControl;
    public AbilityRotation aRotation;

    public PlayerAbilityList(GameManager manager) 
    {
        aSwipeMovement = new AbilitySwipeMovement(manager);
        aRotation = new AbilityRotation(manager);
        //aRocketControl = new AbilityRocketControl();
        //placeObstacle = abilityObjectPlacement();
    }
}
