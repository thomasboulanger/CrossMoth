using TMPro;
using UnityEngine;

public class CrowMovement : EnemyMovement
{
    // Fall animation
   [SerializeField] private AnimationCurve animationCurve;
    private float fallCurrentDuration = 0.0f;
    private float yTarget = 0.5f;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    enum State { Landing, Hunting, Leaving };
    private State state;

    [SerializeField] private float stayDuration = 3.0f;
    private float currentStayDuration = 0.0f;

    private Animator animator;
    public Transform graphics;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        startPosition = graphics.localPosition;
        targetPosition = new Vector3(0.0f, 0.0f, 0.0f);
        state = State.Landing;
    }

    void CrowCrash() {
        if (fallCurrentDuration < 1.0f) {
            Vector3 pos = Vector3.LerpUnclamped(startPosition, targetPosition, animationCurve.Evaluate(fallCurrentDuration));
            Debug.Log(pos);
            fallCurrentDuration += Time.deltaTime;
            graphics.localPosition = pos;
        } else {
            state = State.Hunting;
        }
    }

    protected override void Update()
    {
        if (state == State.Landing) CrowCrash();
        if (state == State.Hunting) currentStayDuration += Time.deltaTime;
    }
}
