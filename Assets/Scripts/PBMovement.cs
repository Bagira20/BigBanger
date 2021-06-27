using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheBigBanger.GameplayStatics;

public enum PBMovementTypes
{
    linear, lerp, radiant
}

public class PBMovement : PlanetMovementBase
{
    public PBMovementTypes movementType = PBMovementTypes.linear;
    public Vector3 targetPos, centerRadiusPos;
    public float radiusSize;
    public bool bMoveAtStart = true;

    private void Update()
    {
        if (bMoveAtStart)
            bIsMoving = true;
        if (bIsMoving)
            UpdateMovePlanet();
    }

    protected override void UpdateMovePlanet()
    {
        targetPos = transform.position+new Vector3(0,100f,0);
        float speed = acceleration != default ? acceleration : velocity;
        switch (movementType) {
            case PBMovementTypes.linear:
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * GameTime.deltaTime); break;
            case PBMovementTypes.lerp:
            case PBMovementTypes.radiant:
                //not yet implemented
                break;
        }
    }

    public float GetForce()
    {
        return acceleration != default ? mass * acceleration : 0.5f * mass * Mathf.Pow(velocity, 2); ;
    }
}
