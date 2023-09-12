using TMPro;
using UnityEngine;

public class CrowMovement : EnemyMovement
{
    // Fall animation
    private float fallSpeedPercentage = 0;    // 0-1 from no speed to full speed
    private float fallSpeed = 30f;              // speed
    private float acceleration = 2f;        // accelerate to full speed in 2 seconds (1 / accel)
    private float yTarget = 0.5f;
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
        targetPosition = new Vector3(graphics.position.x, yTarget, graphics.position.z);
        state = State.Landing;
    }

    void CrowCrash() {
        if (graphics.position.y > yTarget) {
            fallSpeedPercentage += Mathf.Min(acceleration * Time.deltaTime, 1);    // limit to 1 for "full speed"
            Vector3 pos = Vector3.MoveTowards(graphics.position, targetPosition, fallSpeed * fallSpeedPercentage * Time.deltaTime);
            graphics.position = pos;
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
