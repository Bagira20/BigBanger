using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheBigBanger.GameModes;
using TheBigBanger.PlayerStatics;

public class ModeLevel : GameModeBase
{
    public float bTimeLimit = 200f;
    public bool bTimeOver = false;

    public ModeLevel(GameManager manager) : base(manager) { }

    public bool IsTimeOver()
    {
        return GameTime.gameTime > bTimeLimit;
    }

    //score

    //gameover
}
