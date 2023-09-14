using UnityEngine;

public class HealObject : MonoBehaviour
{
    [SerializeField] private float healAmount = 1.0f;
    [SerializeField] private GameObject flowerTop;

    private bool used;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player" && !used) {
            MothHealth mh = other.gameObject.GetComponent<MothHealth>();
            if (mh.currentHealth < mh.maxHealth) {
                mh.Heal(healAmount);
                used = true;
                Destroy(flowerTop);
            }
        }
    }
}
