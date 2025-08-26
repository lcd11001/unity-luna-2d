using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private float verticalLandingThreshold = 0.9f; // Threshold for vertical landing

    [SerializeField]
    private float fuelAmountMax = 10f; // Amount of fuel available
    [SerializeField]
    private float fuelAmount = 0f; // Amount of fuel available
    [SerializeField]
    private float fuelConsumptionAmount = 1f; // Fuel consumption when moving

    [SerializeField]
    private LandedEventChannelSO landedEventChannel;

    #region Events

    public event EventHandler OnUpForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnBeforeForce;

    #endregion

    private Rigidbody2D rb;
    private bool moveUp = false;
    private bool moveLeft = false;
    private bool moveRight = false;

    private bool hasInput = false;

    public static Lander Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            rb = GetComponent<Rigidbody2D>();
            fuelAmount = fuelAmountMax;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);
        if (fuelAmount <= 0)
        {
            // Debug.Log("Out of fuel!");
            return;
        }

        if (moveUp || moveLeft || moveRight)
        {
            hasInput = true;
            ConsumeFuel();
        }

        if (moveUp)
        {
            // Debug.Log("Moving Up");
            rb.AddForce(transform.up * speed * Time.deltaTime, ForceMode2D.Force);
            OnUpForce?.Invoke(this, EventArgs.Empty);
        }

        if (moveLeft)
        {
            // Debug.Log("Moving Left");
            rb.AddTorque(rotationSpeed * Time.deltaTime);
            OnLeftForce?.Invoke(this, EventArgs.Empty);
        }

        if (moveRight)
        {
            // Debug.Log("Moving Right");
            rb.AddTorque(-rotationSpeed * Time.deltaTime);
            OnRightForce?.Invoke(this, EventArgs.Empty);
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
        moveUp = Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed;
        moveLeft = Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed;
        moveRight = Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed;

    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (!hasInput)
        {
            // Debug.Log("No input detected, ignoring collision.");
            return;
        }

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
                    landingDotVector = 0,
                    landingSpeed = 0
                });
            }
            return;
        }

        // Debug.Log("Landing on: " + landingPad.gameObject.name);

        float landingSpeed = collision2D.relativeVelocity.magnitude;
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
                    landingDotVector = 0,
                    landingSpeed = landingSpeed
                });
            }
            return;
        }


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
                    landingDotVector = landingDotVector,
                    landingSpeed = landingSpeed
                });
            }
            return;
        }

        Debug.Log("Landing successful!");

        float maxAngleScore = 100f;
        // Map dot from [verticalLandingThreshold .. 1] -> [0 .. 1]
        float angleNormalized = Mathf.InverseLerp(verticalLandingThreshold, 1f, landingDotVector);
        float angleScore = angleNormalized * maxAngleScore;
        // Debug.Log("angleScore: " + angleScore);

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
                landingDotVector = landingDotVector,
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
}
