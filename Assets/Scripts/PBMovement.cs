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
    public Vector3 linearDirection = Vector3.up, centerFromPlanet = Vector3.zero, radiantAxis = Vector3.right;
    public float yLerpOffset = 1f;
    protected Vector3 radiantCenter, lerpTarget, lerpStart;
    protected float lerpTimeCounter, lerpTimeAtFirstLaunch;
    public bool bMoveAtStart = true;

    private void OnEnable()
    {
        radiantCenter = transform.position + centerFromPlanet;
        lerpStart = transform.position;
        //Only UP MOVEMENT so far
        lerpTarget = new Vector3(lerpStart.x, lerpStart.y + yLerpOffset, lerpStart.z);
    }

    private void Update()
    {
        if (bMoveAtStart)
        {
            bIsMoving = true;
            bMoveAtStart = false;
        }
        if (bIsMoving)
            UpdateMovePlanet();
    }

    protected override void UpdateMovePlanet()
    {
        float speed = acceleration != default ? acceleration : velocity;
        switch (movementType) {
            case PBMovementTypes.linear:
                transform.position += (linearDirection * speed) * GameTime.deltaTime; 
                break;
            case PBMovementTypes.radiant:
                transform.RotateAround(radiantCenter, radiantAxis, speed*GameTime.deltaTime);
                break;
            case PBMovementTypes.lerp:
                if (GameTime.bFreeze) return;
                lerpTimeCounter += GameTime.deltaTime;
                float theta = lerpTimeCounter * velocity;
                float distance = (lerpTarget.y - lerpStart.y) * Mathf.Sin(theta);
                transform.position = lerpStart + new Vector3(0, yLerpOffset) * distance;
                break;
        }
    }

    private void FixedUpdate()
    {
        canvas.SetTargetText(manager.GetTransformedValue(mass), manager.GetTransformedValue(velocity));
        canvas.AttachTextToObject(EUIElements.TargetText, this.gameObject);
    }

    public override void LaunchPlanet()
    {
        base.LaunchPlanet();
        if (!manager.activeMode.bFirstLaunch)
        {
            manager.activeMode.bFirstLaunch = true;
            startPos = transform.position;
            lerpTimeAtFirstLaunch = lerpTimeCounter;
        }
    }

    public float GetForce()
    {
        return acceleration != default ? mass * acceleration : 0.5f * mass * Mathf.Pow(velocity, 2); ;
    }

    public override void ResetPlanet()
    {
        base.ResetPlanet();
        lerpTimeCounter = lerpTimeAtFirstLaunch;
    }
}
