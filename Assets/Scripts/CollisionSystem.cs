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
            gameManager.DebugText.text = "HIT!";

            if (playerMovement.GetForceFromAbility(playerMovement.planetVelocityBy) > targetMovement.GetForce())
            {
                //Game Over
                gameManager.activeMode.gamePhase = EGamePhase.LevelEnd;
                gameManager.activeMode.gamepass = 1;
                gameManager.levelEndCanvas.GetComponentInChildren<Text>().text = "You've Completed the Level!\nFORCE: " 
                    + gameManager.GetTransformedValue(playerMovement.GetForceFromAbility(playerMovement.planetVelocityBy)) 
                    + "\nVELOCITY: " + gameManager.GetTransformedValue(playerMovement.GetVelocityFromAbility(playerMovement.planetVelocityBy)) 
                    + "\nMASS: " + gameManager.GetTransformedValue(playerMovement.GetMass()) + "\n\n" + LevelIntroDisplays.LevelIntroText[gameManager.levelIntroNr];
                gameManager.DebugText.text = "HIT with Force!";
                playerMovement.DestroyPlanet();
                targetMovement.DestroyPlanet();
            }
        }
    }
}
