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

    // Forward rotation when turning
    [SerializeField] private float rotationSpeed = 10.0f;
    [SerializeField] private float rotationAmplitude = 1;
    private Vector3 lastSavedForward;
    private float lerpedDiffAngle = 0;
    
    // Components
    [SerializeField] private Animator animator;
    public Transform graphics;
    public Transform crowRotateTransform;

    //Particules
    [SerializeField] private GameObject explosionParticule;

    //Sounds
    [SerializeField] private AudioClip[] audioClips;
    private AudioSource audioSource;

    protected override void Start()
    {
        base.Start();

        audioSource = GetComponent<AudioSource>();
        landingStartPosition = graphics.localPosition;
        state = State.Landing;
        animator.SetBool("isFlying", false);
    }

    protected override void Update()
    {
        if (state == State.Landing) CrowCrash();
        if (state == State.Hunting) {
            // Forward rotation
            float forwardDiffAngle = Vector3.SignedAngle(lastSavedForward, crowRotateTransform.forward, crowRotateTransform.up) * rotationAmplitude;
            lerpedDiffAngle = Mathf.Lerp(lerpedDiffAngle, forwardDiffAngle, Time.deltaTime * rotationSpeed);
            crowRotateTransform.localRotation = Quaternion.Euler(crowRotateTransform.localRotation.eulerAngles.x, crowRotateTransform.localRotation.eulerAngles.y, lerpedDiffAngle);
            lastSavedForward = crowRotateTransform.forward;
            
            // Chasing
            base.MoveTowardsPlayer();
            // Leaving ?
            currentStayDuration += Time.deltaTime;
            if (currentStayDuration >= stayDuration) {
                StartCrowLeave();
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

            //sounds
            audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
            audioSource.Play();
        }
    }

    void StartCrowLeave() {
        // Flips top-down of the 3d object
        graphics.localScale = new Vector3(graphics.localScale.x, -graphics.localScale.y, graphics.localScale.z);

        canAttack = false;
        state = State.Leaving;
        leavingStartPosition = graphics.localPosition;
        UpdateAnimationState(false);

        //sounds
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.Play();
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
            StartCrowLeave();
        }
    }

    void UpdateAnimationState(bool status)
    {
        animator.SetBool("isFlying", status);
        GameObject go = Instantiate(explosionParticule, transform.position, Quaternion.identity);
        Destroy(go, 5);
    }
}
