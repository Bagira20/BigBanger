using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityRotation : AbilityBase
{
    public bool bInputLocked;
    float _sensitivity, _distanceToPlanet, _touchDistance;
    Vector3 _rotateAxis = Vector3.up, _center;
    Vector2 _startTouchPos, _currentTouchPos, _previousTouchPos;
    int _touchDirection = 1;

    public AbilityRotation(GameManager manager) : base(manager)
    {
    }

    public void StartRotation(AbilityBase ability)
    {
        bInputLocked = true;
        _distanceToPlanet = Mathf.Abs(Vector2.Distance(playerMovement.transform.position, ability.inputCursor.transform.position));
        _sensitivity = playerMovement.rotationSensitivity / _distanceToPlanet;
        _center = playerMovement.transform.position;
        _startTouchPos = _currentTouchPos = TouchInput.GetTouchPosition();
        AudioPlayer.Play2DAudioFromRange(playerMovement.audioSource, gameManager.canvas.TouchSounds, new Vector2(0.7f, 0.9f), new Vector2(0.2f, 0.25f));
    }

    public void UpdateRotation(AbilityBase ability)
    {
        UpdateTouchDistance();
        ability.inputCursor.transform.RotateAround(_center, _rotateAxis, _sensitivity * _touchDistance);
        gameManager.activeMode.aSwipeMovement.SetLinePositions(ability.inputCursor.transform.position);
        gameManager.activeMode.aSwipeMovement.UpdateSwipeData();
    }

    void UpdateTouchDistance()
    {
        _previousTouchPos = _currentTouchPos;
        _currentTouchPos = TouchInput.GetTouchPosition();
        _touchDirection = _previousTouchPos.x < _currentTouchPos.x ? 1 : -1;
        _touchDistance = _touchDirection * Mathf.Abs(_previousTouchPos.x - _currentTouchPos.x);
    }

    public void EndRotation(AbilityBase ability)
    {
        AudioPlayer.Play2DAudioFromRange(playerMovement.audioSource, gameManager.canvas.CancelSounds, new Vector2(0.7f, 0.9f), new Vector2(0.2f, 0.25f));
        bInputLocked = false;
    }
}