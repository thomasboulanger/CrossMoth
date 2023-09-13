using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class HealObject : MonoBehaviour
{
    [SerializeField] private float healAmount = 1.0f;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<MothHealth>().Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
