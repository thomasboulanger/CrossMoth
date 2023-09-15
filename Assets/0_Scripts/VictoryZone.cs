using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryZone : MonoBehaviour
{
    public HighScore highScore;

    private void Start() {
        highScore = GameObject.FindGameObjectWithTag("HighScore").GetComponent<HighScore>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            highScore.GameEnded(true);
            Time.timeScale = 0.0f;
        }
    }
}
