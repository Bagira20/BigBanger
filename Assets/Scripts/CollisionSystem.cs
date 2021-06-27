using System.Collections;
using System.Collections.Generic;
using TheBigBanger.GameModeManager;
using UnityEngine;
using UnityEngine.UI;

public class CollisionSystem : MonoBehaviour
{
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider collidedGameObject)
    {
        if (collidedGameObject.name == "TargetPlanet")
        {
            //check for force
            PAMovement playerMovement = gameManager.playerGameObject.GetComponent<PAMovement>();
            PBMovement targetMovement = collidedGameObject.GetComponent<PBMovement>();
            if (playerMovement.GetForceFromAbility(PlayerAbilityList.playerAbilities.swipeMovement) > targetMovement.GetForce())
            {
                gameManager.activeMode.gamePhase = GamePhase.LevelEnd;
                gameManager.levelEndCanvas.GetComponentInChildren<Text>().text = "You've Completed the Level!\nFORCE: " + playerMovement.GetForceFromAbility(PlayerAbilityList.playerAbilities.swipeMovement) + "\nVELOCITY: " + playerMovement.GetVelocityFromAbility(PlayerAbilityList.playerAbilities.swipeMovement) + "\nMASS: " + playerMovement.GetMass() + "\n\nF = 1/2*m*(v²)";
                Debug.Log("collided with target planet");
            }
        }
    }
}
