using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControlAbilities : MonoBehaviour
{
    /*non-static library for all abilities*/
    public AbilitySwipeMovement aSwipeMovement;
    public AbilityRocketControl aRocketControl;

    public GameControlAbilities(GameManager manager) 
    {
        aSwipeMovement = new AbilitySwipeMovement(manager);
        //aRocketControl = new AbilityRocketControl();
    }
}
