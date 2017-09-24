using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class GM : MonoBehaviour {

    public static GM instance;

    public Transform playerPrefab;
    public Transform spawParticlePrefab;
    public Transform spawnPoint;

    public float respawnWaitTime = 1.5f;
    public float respawnParticlesWaitTime = 3f;
    public float cameraPositioningWaitTime = 1.5f;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void RespawnPlayer() {
        Camera.main.gameObject.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y, Camera.main.gameObject.transform.position.z);
        Camera.main.GetComponent<Camera2DFollow>().target = spawnPoint;
        Invoke("SpawnWithEffects", cameraPositioningWaitTime);
    }

    public void KillPlayer(Player player) {
        Destroy(player.gameObject);
        Invoke("RespawnPlayer", respawnWaitTime);
    }

    void SpawnWithEffects() {
        Transform newPlayer = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
        Transform particles = Instantiate(spawParticlePrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
        Camera.main.GetComponent<Camera2DFollow>().target = newPlayer;
        Destroy(particles.gameObject, respawnParticlesWaitTime);
    }
}
