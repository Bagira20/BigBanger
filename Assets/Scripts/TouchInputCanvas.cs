using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputCanvas : MonoBehaviour
{
    GameObject _playerObject, _arCamera;
    Vector3 _orientationTarget;
    GameManager manager;
    void Start()
    {
        manager = GameObject.Find("/GameManager").GetComponent<GameManager>();
        _playerObject = manager.playerGameObject;
        _arCamera = manager.arCamera.gameObject;
        UpdateTransform();
    }

    void Update()
    {
        UpdateTransform();
    }

    void UpdateTransform() 
    {
        transform.parent.position = _playerObject.transform.position;
    }
}
