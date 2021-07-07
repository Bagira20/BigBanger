using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityRotation : AbilityBase
{
    public bool bInputLocked;
    float _sensitivity, _distanceToPlanet, _touchDistance;
    Vector3 _rotateAxis = Vector3.up, _center;
    Vector2 _startTouchPos, _currentTouchPos;
    LineRenderer _playerLine;
    PAMovement _playerMovement;

    public AbilityRotation(GameManager manager) : base(manager)
    {
        _playerMovement = manager.playerGameObject.GetComponent<PAMovement>();
        _playerLine = _playerMovement.lineRenderer;
    }

    public void StartRotation(AbilityBase ability)
    {
        bInputLocked = true;
        _distanceToPlanet = Mathf.Abs(Vector2.Distance(_playerMovement.transform.position, ability.inputCursor.transform.position));
        _sensitivity = _playerMovement.rotationSensitivity / _distanceToPlanet;
        _center = new Vector3(_playerMovement.transform.position.x, ability.inputCursor.transform.position.y, _playerMovement.transform.position.z);
        _startTouchPos = TouchInput.GetTouchPosition();
    }

    public void UpdateRotation(AbilityBase ability)
    {
        ability.inputCursor.transform.RotateAround(_center, _rotateAxis, _sensitivity * GetTouchDistance());
        _playerLine.SetPosition(1, ability.inputCursor.transform.position);
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