using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityRotation : AbilityBase
{
    public bool bInputLocked;
    float _sensitivity, _distanceToPlanet, _touchDistance;
    Vector3 _rotateAxis = Vector3.up, _center;
    Vector2 _startTouchPos, _currentTouchPos;

    public AbilityRotation(GameManager manager) : base(manager)
    {
    }

    public void StartRotation(AbilityBase ability)
    {
        bInputLocked = true;
        _distanceToPlanet = Mathf.Abs(Vector2.Distance(playerMovement.transform.position, ability.inputCursor.transform.position));
        _sensitivity = playerMovement.rotationSensitivity / _distanceToPlanet;
        _center = playerMovement.transform.position;
        _startTouchPos = TouchInput.GetTouchPosition();
    }

    public void UpdateRotation(AbilityBase ability)
    {
        ability.inputCursor.transform.RotateAround(_center, _rotateAxis, _sensitivity * GetTouchDistance());
        gameManager.activeMode.aSwipeMovement.SetLinePositions(ability.inputCursor.transform.position);
        gameManager.activeMode.aSwipeMovement.UpdateSwipeData();
    }

    float GetTouchDistance()
    {
        _currentTouchPos = TouchInput.GetTouchPosition();
        _touchDistance = _startTouchPos.x - _currentTouchPos.x;
        return _touchDistance;
    }

    public void EndRotation(AbilityBase ability)
    {
        bInputLocked = false;
    }
}