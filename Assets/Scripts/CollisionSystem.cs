using System.Collections;
using System.Collections.Generic;
using TheBigBanger.GameModeManager;
using UnityEngine;
using UnityEngine.UI;

public class CollisionSystem : MonoBehaviour
{
    GameManager gameManager;
    ParticleSystem collisionExplosion;
    PAMovement playerMovement;
    PBMovement targetMovement;

    private void Start()
    {
        collisionExplosion = GameObject.Find("ExplosionCore").GetComponent<ParticleSystem>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerMovement = gameManager.playerGameObject.GetComponent<PAMovement>();
        targetMovement = gameManager.targetGameObject.GetComponent<PBMovement>();
    }

    private void OnTriggerEnter(Collider collidedGameObject)
    {
        Debug.Log("HIT!");
        if (collidedGameObject.name == "TargetPlanet")
        {
            Debug.Log("HIT TARGET!");

            if (playerMovement.GetForceFromAbility(playerMovement.planetVelocityBy) > targetMovement.GetForce())
            {
                playerMovement.audioSource.Stop();
                AudioSource.PlayClipAtPoint(playerMovement.ExplosionSounds[Random.Range(0, playerMovement.ExplosionSounds.Length - 1)], playerMovement.transform.position);
                StartCoroutine(StartGameOver());
            }
        }
    }

    IEnumerator StartGameOver()
    {
        collisionExplosion.gameObject.transform.position = gameManager.targetGameObject.transform.position;
        collisionExplosion.gameObject.transform.position += new Vector3(0, 0, 0.2f);
        collisionExplosion.Play(true);
        Debug.Log("explosion started");
        playerMovement.DestroyPlanet();
        targetMovement.DestroyPlanet();

        yield return new WaitForSeconds(5);

        collisionExplosion.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        gameManager.activeMode.gamePhase = EGamePhase.LevelEnd;
        gameManager.activeMode.gamepass = 1;
        gameManager.playerScoreStats.text = "FORCE: "+ gameManager.GetTransformedValue(playerMovement.GetForceFromAbility(playerMovement.planetVelocityBy))
            + " N\nVELOCITY: " + gameManager.GetTransformedValue(playerMovement.GetVelocityFromAbility(playerMovement.planetVelocityBy))
            + " m/s\nMASS: " + gameManager.GetTransformedValue(playerMovement.GetMass()) + " kg\n\n" + LevelIntroDisplays.LevelIntroText[gameManager.levelIntroNr];
        //gameManager.DebugText.text = "HIT with Force!";
    }
}
