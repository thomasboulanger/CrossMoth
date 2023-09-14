using TMPro;
using UnityEngine;

public class CrowMovement : EnemyMovement
{
    // Attack
    [SerializeField] private float attackDamage = 1.0f;
    private bool canAttack = true;


    // Fall animation
    [SerializeField] private AnimationCurve landingAnimationCurve;
    private float fallCurrentDuration = 0.0f;
    private Vector3 landingStartPosition;

    // Leaving animation
    [SerializeField] private AnimationCurve leavingAnimationCurve;
    private float leavingCurrentDuration = 0.0f;
    private Vector3 leavingStartPosition;

    // States
    enum State { Landing, Hunting, Leaving };
    private State state;

    // Stay duration
    [SerializeField] private float stayDuration = 3.0f;
    private float currentStayDuration = 0.0f;

    // Components
    [SerializeField] private Animator animator;
    public Transform graphics;

    //Particules
    [SerializeField] private GameObject explosionParticule;

    protected override void Start()
    {
        base.Start();

        landingStartPosition = graphics.localPosition;
        state = State.Landing;
        animator.SetBool("isFlying", false);
    }

    protected override void Update()
    {
        if (state == State.Landing) CrowCrash();
        if (state == State.Hunting) {
            base.MoveTowardsPlayer();
            currentStayDuration += Time.deltaTime;
            if (currentStayDuration >= stayDuration) {
                state = State.Leaving;
                leavingStartPosition = graphics.localPosition;
                UpdateAnimationState(false);
            }
        }
        if (state == State.Leaving) {
            CrowLeaving();
        }
    }

    void CrowCrash() {
        if (fallCurrentDuration < 1.0f) {
            Vector3 pos = Vector3.LerpUnclamped(landingStartPosition, new Vector3(0.0f, 0.0f, 0.0f), landingAnimationCurve.Evaluate(fallCurrentDuration));
            fallCurrentDuration += Time.deltaTime;
            graphics.localPosition = pos;
        } else {
            state = State.Hunting;
            UpdateAnimationState(true);
        }
    }

    void CrowLeaving() {
        if (leavingCurrentDuration < 1.0f) {
            Vector3 pos = Vector3.LerpUnclamped(leavingStartPosition, new Vector3(0.0f, 100.0f, 0.0f), leavingAnimationCurve.Evaluate(leavingCurrentDuration));
            leavingCurrentDuration += Time.deltaTime;
            graphics.localPosition = pos;
        } else {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && canAttack) {
            other.GetComponent<MothHealth>().TakeDamage(attackDamage);
            // Making sure the crow leaves after dmg and cannot attack twice
            canAttack = false;
            state = State.Leaving;
            leavingStartPosition = graphics.localPosition;
            UpdateAnimationState(true);
        }
    }

    void UpdateAnimationState(bool status)
    {
        animator.SetBool("isFlying", status);
        Instantiate(explosionParticule, transform.position, Quaternion.identity);
    }
}
