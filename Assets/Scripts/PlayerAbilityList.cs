using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerAbilities { swipeMovement, rocketMovement, everything, none };


public class PlayerAbilityList
{
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
