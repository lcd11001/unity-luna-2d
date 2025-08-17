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

    private Rigidbody2D rb;
    private bool moveUp = false;
    private bool moveLeft = false;
    private bool moveRight = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (moveUp)
        {
            // Debug.Log("Moving Up");
            rb.AddForce(transform.up * speed * Time.deltaTime, ForceMode2D.Force);
        }

        if (moveLeft)
        {
            // Debug.Log("Moving Left");
            rb.AddTorque(rotationSpeed * Time.deltaTime);
        }

        if (moveRight)
        {
            // Debug.Log("Moving Right");
            rb.AddTorque(-rotationSpeed * Time.deltaTime);
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
        if (!collision2D.gameObject.TryGetComponent<LandingPad>(out LandingPad landingPad))
        {
            Debug.Log("Crash on: " + collision2D.gameObject.name);
            return;
        }

        // Debug.Log("Landing on: " + landingPad.gameObject.name);

        float landingSpeed = collision2D.relativeVelocity.magnitude;
        // Debug.Log("Relative Velocity: " + collision2D.relativeVelocity + " magnitude " + landingSpeed);
        if (landingSpeed > softLandingThreshold)
        {
            Debug.Log("Hard landing detected!");
            return;
        }


        float landingDotVector = Vector2.Dot(collision2D.relativeVelocity.normalized, Vector2.up);
        // Debug.Log("Dot Vector: " + landingDotVector);
        if (landingDotVector < verticalLandingThreshold)
        {
            Debug.Log("Lander is not vertical enough!");
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

        float scoreMultiply = 10f;
        float totalScore = Mathf.Floor(angleScore + softLandingScore) * scoreMultiply;
        Debug.Log("Total Score: " + totalScore);
    }
}
