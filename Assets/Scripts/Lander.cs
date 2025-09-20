using System;
using UnityEngine;
// using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Lander : MonoBehaviour
{
    [SerializeField]
    private float speed = 700f; // Speed of the lander
    [SerializeField]
    private float rotationSpeed = 100f; // Speed of rotation
    [SerializeField]
    private float softLandingThreshold = 4f; // Soft landing threshold
    [SerializeField]
    // private float verticalLandingThreshold = 0.9f; // Threshold for vertical landing
    private float verticalLandingAngleThreshold = 15f; // Threshold for vertical landing angle

    [SerializeField]
    private float fuelAmountMax = 10f; // Amount of fuel available
    [SerializeField]
    private float fuelAmount = 0f; // Amount of fuel available
    [SerializeField]
    private float fuelConsumptionAmount = 1f; // Fuel consumption when moving

    [SerializeField]
    private LandedEventChannelSO landedEventChannel;

    [SerializeField]
    private LanderStateEventChannelSO stateEventChannel;

    #region Events

    public event EventHandler OnUpForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler OnExplosion;

    #endregion

    private Rigidbody2D rb;
    private bool moveUp = false;
    private bool moveLeft = false;
    private bool moveRight = false;
    private Vector2 movementInput = Vector2.zero;

    public static Lander Instance { get; private set; }

    private const float GRAVITY_NORMAL = 0.7f;
    private const float MOVEMENT_THRESHOLD = 0.1f;

    public enum State
    {
        Waiting,
        Flying,
        Landing
    }

    private State currentState;
    public State GetState() { return currentState; }
    private void SetState(State newState)
    {
        currentState = newState;
        stateEventChannel.RaiseEvent(newState);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;

            fuelAmount = fuelAmountMax;

            SetState(State.Waiting);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);

        switch (GetState())
        {
            case State.Waiting:
                if (moveUp || moveLeft || moveRight || movementInput.magnitude > MOVEMENT_THRESHOLD)
                {
                    rb.gravityScale = GRAVITY_NORMAL;

                    SetState(State.Flying);
                }
                break;
            case State.Flying:
                if (fuelAmount <= 0)
                {
                    // Debug.Log("Out of fuel!");
                    return;
                }

                if (moveUp || moveLeft || moveRight || movementInput.magnitude > MOVEMENT_THRESHOLD)
                {
                    ConsumeFuel();
                }

                if (moveUp || movementInput.y > MOVEMENT_THRESHOLD)
                {
                    // Debug.Log("Moving Up");
                    rb.AddForce(transform.up * speed * Time.deltaTime, ForceMode2D.Force);
                    OnUpForce?.Invoke(this, EventArgs.Empty);
                }

                if (moveLeft || movementInput.x < -MOVEMENT_THRESHOLD)
                {
                    // Debug.Log("Moving Left");
                    rb.AddTorque(rotationSpeed * Time.deltaTime);
                    OnLeftForce?.Invoke(this, EventArgs.Empty);
                }

                if (moveRight || movementInput.x > MOVEMENT_THRESHOLD)
                {
                    // Debug.Log("Moving Right");
                    rb.AddTorque(-rotationSpeed * Time.deltaTime);
                    OnRightForce?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.Landing:
                break;
        }
    }

    private void ConsumeFuel()
    {
        if (fuelAmount > 0)
        {
            fuelAmount -= fuelConsumptionAmount * Time.deltaTime;
            fuelAmount = Mathf.Max(fuelAmount, 0);
        }
    }

    private void Update()
    {
        // moveUp = Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed;
        // moveLeft = Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed;
        // moveRight = Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed;

        moveUp = GameInput.Instance.IsUpActionPressed();
        moveLeft = GameInput.Instance.IsLeftActionPressed();
        moveRight = GameInput.Instance.IsRightActionPressed();

        movementInput = GameInput.Instance.GetMovementVector();
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (GetState() != State.Flying)
        {
            Debug.Log("current state not flying, ignoring collision.");
            return;
        }

        SetState(State.Landing);
        float landingSpeed = collision2D.relativeVelocity.magnitude;
        float landingAngle = Vector2.Angle(transform.up, Vector2.up);

        if (!collision2D.gameObject.TryGetComponent<LandingPad>(out LandingPad landingPad))
        {
            Debug.Log("Crash on: " + collision2D.gameObject.name);
            if (landedEventChannel != null)
            {
                landedEventChannel.RaiseEvent(new OnLandingEvent
                {
                    type = LandingType.WrongLandingArea,
                    scoreMultiplier = 0,
                    score = 0,
                    landingAngle = landingAngle,
                    landingSpeed = landingSpeed
                });
            }
            Explosion();
            return;
        }

        // Debug.Log("Landing on: " + landingPad.gameObject.name);



        // Debug.Log("Relative Velocity: " + collision2D.relativeVelocity + " magnitude " + landingSpeed);
        if (landingSpeed > softLandingThreshold)
        {
            Debug.Log("Hard landing detected!");
            if (landedEventChannel != null)
            {
                landedEventChannel.RaiseEvent(new OnLandingEvent
                {
                    type = LandingType.TooFastLanding,
                    scoreMultiplier = landingPad.ScoreMultiplier,
                    score = 0,
                    landingAngle = landingAngle,
                    landingSpeed = landingSpeed
                });
            }
            Explosion();
            return;
        }

        /*
        float landingDotVector = Vector2.Dot(collision2D.relativeVelocity.normalized, Vector2.up);
        // Debug.Log("Dot Vector: " + landingDotVector);
        if (landingDotVector < verticalLandingThreshold)
        {
            Debug.Log("Lander is not vertical enough!");
            if (landedEventChannel != null)
            {
                landedEventChannel.RaiseEvent(new OnLandingEvent
                {
                    type = LandingType.TooSteepAngle,
                    scoreMultiplier = landingPad.ScoreMultiplier,
                    score = 0,
                    landingAngle = landingDotVector,
                    landingSpeed = landingSpeed
                });
            }
            Explosion();
            return;
        }
        */

        Debug.Log("Landing angle: " + landingAngle);
        if (landingAngle > verticalLandingAngleThreshold)
        {
            Debug.Log("Lander is not vertical enough!");
            if (landedEventChannel != null)
            {
                landedEventChannel.RaiseEvent(new OnLandingEvent
                {
                    type = LandingType.TooSteepAngle,
                    scoreMultiplier = landingPad.ScoreMultiplier,
                    score = 0,
                    landingAngle = landingAngle,
                    landingSpeed = landingSpeed
                });
            }
            Explosion();
            return;
        }

        Debug.Log("Landing successful!");

        float maxAngleScore = 100f;
        // Map dot from [verticalLandingThreshold .. 1] -> [0 .. 1]
        // float angleNormalized = Mathf.InverseLerp(verticalLandingThreshold, 1f, landingDotVector);
        float angleNormalized = Mathf.InverseLerp(verticalLandingAngleThreshold, 0f, landingAngle);
        float angleScore = angleNormalized * maxAngleScore;
        Debug.Log("angleScore: " + angleScore);

        float maxSoftLandingScore = 100f;
        float speedNormalized = Mathf.InverseLerp(0f, softLandingThreshold, landingSpeed);
        float softLandingScore = (1.0f - speedNormalized) * maxSoftLandingScore;
        // Debug.Log("softLandingScore: " + softLandingScore);

        int totalScore = Mathf.FloorToInt(angleScore + softLandingScore) * landingPad.ScoreMultiplier;
        Debug.Log("Total Score: " + totalScore);

        if (landedEventChannel != null)
        {
            landedEventChannel.RaiseEvent(new OnLandingEvent
            {
                type = LandingType.Success,
                scoreMultiplier = landingPad.ScoreMultiplier,
                score = totalScore,
                // landingAngle = landingDotVector,
                landingAngle = landingAngle,
                landingSpeed = landingSpeed
            });
        }
    }

    public void HandleFuelPickedUp(float fuelAmount)
    {
        this.fuelAmount += fuelAmount;
        this.fuelAmount = Mathf.Min(this.fuelAmount, fuelAmountMax);
        Debug.Log("Added fuel: " + fuelAmount + ", new total: " + this.fuelAmount);
    }

    public float GetFuelAmount()
    {
        return fuelAmount;
    }

    public float GetFuelAmountMax()
    {
        return fuelAmountMax;
    }

    public float GetFuelAmountNormalized()
    {
        return fuelAmount / fuelAmountMax;
    }

    public float GetSpeedX()
    {
        return rb.linearVelocity.x;
    }

    public float GetSpeedY()
    {
        return rb.linearVelocity.y;
    }

    private void Explosion()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        OnExplosion?.Invoke(this, EventArgs.Empty);
    }
}
