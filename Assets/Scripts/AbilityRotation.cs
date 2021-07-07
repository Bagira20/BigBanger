using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityRotation : AbilityBase
{
    float radius;
    Vector2 startPos, tempPos, newPos;
    LineRenderer playerLine;

    public AbilityRotation(GameManager manager) : base(manager)
    {
        playerLine = manager.playerGameObject.GetComponent<PAMovement>().lineRenderer;
    }

    public void StartRotation()
    {
        Vector2 lineEndPos = new Vector2(playerLine.GetPosition(1).x, playerLine.GetPosition(1).z);
        Vector2 lineOriginpos = new Vector2(playerLine.GetPosition(0).x, playerLine.GetPosition(0).z);
        radius = Mathf.Abs(Vector2.Distance(lineEndPos, lineOriginpos));
    }

    public void UpdateRotation()
    {

    }

    public void EndRotation()
    {
        playerLine.SetPosition(1, newPos = tempPos);
    }
}
