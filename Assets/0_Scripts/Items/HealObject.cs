using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealObject : MonoBehaviour
{
    [SerializeField] private float healAmount = 1.0f;
    [SerializeField] private GameObject flowerTop;
    [SerializeField] private GameObject flowerExplosion;

    private Animator animator;

    private bool used;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player" && !used) {
            MothHealth mh = other.gameObject.GetComponent<MothHealth>();
            if (mh.currentHealth < mh.maxHealth) {
                mh.Heal(healAmount);
                used = true;

                animator.SetTrigger("destroy");
                StartCoroutine(DelayAfterDestroy());
                Destroy(Instantiate(flowerExplosion, flowerTop.transform.position, Quaternion.identity), 5f);
            }
        }
    }


    IEnumerator DelayAfterDestroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(flowerTop);
    }

}
