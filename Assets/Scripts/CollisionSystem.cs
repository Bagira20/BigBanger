using System.Collections;
using System.Collections.Generic;
using TheBigBanger.GameModeManager;
using UnityEngine;

public class CollisionSystem : MonoBehaviour
{
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnCollisionEnter(Collision collidedGameObject)
    {
        if (collidedGameObject.collider.name == "TargetPlanet")
        {
            gameManager.activeMode.gamePhase = GamePhase.LevelEnd;
            Debug.Log("collided with target planet");
        }
    }
}
