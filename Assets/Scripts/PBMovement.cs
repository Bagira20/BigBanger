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
    //public Vector3 targetPos, centerRadiusPos;
    //public float radiusSize;
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
        //if (bIsMoving)
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
                float theta = Time.time * velocity;
                float distance = (lerpTarget.y - lerpStart.y) * Mathf.Sin(theta);
                transform.position = lerpStart + new Vector3(0, yLerpOffset, 0) * distance;
                //transform.position = Vector3.Lerp(lerpStart, lerpTarget, Mathf.Abs(Mathf.Sin(Time.time)));
                break;
        }
    }

    private void FixedUpdate()
    {
        canvas.AttachTextToObject(EUIElements.TargetText, this.gameObject);
    }

    public override void LaunchPlanet()
    {
        base.LaunchPlanet();
        if (!manager.activeMode.bFirstLaunch)
        {
            manager.activeMode.bFirstLaunch = true;
            startPos = transform.position;
        }
    }

    public float GetForce()
    {
        return acceleration != default ? mass * acceleration : 0.5f * mass * Mathf.Pow(velocity, 2); ;
    }
}
