using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class GM : MonoBehaviour {

    public static GM instance;

    public Transform playerPrefab;
    public Transform spawnPoint;

    public float respawnWaitTime = 2f;

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
        Transform newPlayer = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
        Camera.main.GetComponent<Camera2DFollow>().target = newPlayer;
    }

    public void KillPlayer(Player player) {
        Destroy(player.gameObject);
        Invoke("RespawnPlayer", respawnWaitTime);
    }
}
