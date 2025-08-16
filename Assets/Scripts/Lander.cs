using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Lander : MonoBehaviour
{
    [SerializeField]
    private float speed = 700f; // Speed of the lander
    [SerializeField]
    private float rotationSpeed = 100f; // Speed of rotation

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
}
