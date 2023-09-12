using TMPro;
using UnityEngine;

public class CrowMovement : EnemyMovement
{
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
    private Animator animator;
    public Transform graphics;

    protected override void Start()
    {
        base.Start();
        //animator = GetComponent<Animator>();
        landingStartPosition = graphics.localPosition;
        state = State.Landing;
        //animator.SetTrigger("Falling");
    }

    protected override void Update()
    {
        if (state == State.Landing) CrowCrash();
        if (state == State.Hunting) {
            base.MoveTowardsPlayer();
            currentStayDuration += Time.deltaTime;
            if (currentStayDuration >= stayDuration) {
                state = State.Leaving;
                //animator.SetTrigger("Leaving");
            }
        }
        if (state == State.Leaving) {
            leavingStartPosition = graphics.localPosition;
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
            //animator.SetTrigger("Hunting");
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
}
