using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheBigBanger.GameModes;
using TheBigBanger.PlayerStatics;

public class ModeScenario : GameModeBase
{
    public float bTimeLimit = 200f;
    public bool bTimeOver = false;

    public ModeScenario(GameManager manager) : base(manager) { }

    public bool IsTimeOver()
    {
        return PlayerTime.gameTime > bTimeLimit;
    }
}
