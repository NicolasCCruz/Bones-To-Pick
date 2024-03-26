using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public Sprite sprite1; // Assign Jean 1_1 (idle) in Inspector
    public Sprite sprite2; // Assign movement sprite in Inspector
    public Sprite sprite3; // Assign alternate movement sprite in Inspector
    public Sprite controlSprite; // Assign the special Control sprite in Inspector
    private SpriteRenderer spriteRenderer;
    public float speed = 5.0f; // Movement speed, adjustable in Inspector
    private float changeSpriteInterval = 0.1f; // Time between sprite changes
    private float spriteTimer;
    private bool isMoving = false; // Indicates if the player is moving
    private bool isControlActive = false; // Control sprite activation flag
    private int currentSpriteIndex = 2; // Current sprite index for toggling between sprite2 and sprite3
    private float controlTimer = 0f; // Timer for control sprite duration
    private float inactivityTimer = 0f; // Timer to track movement inactivity

    // New variables for text interaction
    public TextMeshProUGUI textBox; // Assign your TextMeshProUGUI component in the Inspector
    private bool allowMovement = false; // Control movement based on text display
    private bool awaitingWASDInput = false; // To check if we are waiting for the player to move

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite1; // Start with idle sprite
        DisplayInitialText(); // Display the initial text
    }

    void Update()
    {
        HandleTextInteraction(); // Handle text display and interaction logic

        if (allowMovement)
        {
            // Your existing movement and sprite animation logic
            Vector3 moveDirection = Vector3.zero;
            isMoving = false; // Reset moving flag

            if (Input.GetKey(KeyCode.D))
            {
                moveDirection += Vector3.right;
                isMoving = true;
            }
            if (Input.GetKey(KeyCode.A))
            {
                moveDirection += Vector3.left;
                isMoving = true;
            }
            if (Input.GetKey(KeyCode.W))
            {
                moveDirection += Vector3.up;
                isMoving = true;
            }
            if (Input.GetKey(KeyCode.S))
            {
                moveDirection += Vector3.down;
                isMoving = true;
            }

            if (isMoving)
            {
                AnimateMovementSprite();
            }
            else
            {
                // If not moving and not in control mode, revert to idle sprite after inactivity
                if (!isControlActive)
                {
                    inactivityTimer += Time.deltaTime;
                    if (inactivityTimer >= 0.5f)
                    {
                        spriteRenderer.sprite = sprite1;
                        inactivityTimer = 0f;
                    }
                }
            }

            transform.position += moveDirection * speed * Time.deltaTime;
        }
    }

    void DisplayInitialText()
    {
        textBox.text = "Hi it is I, your astral self. I am here to assist you through your journey. Press enter";
        allowMovement = false; // Prevent movement when text is displayed
    }

void HandleTextInteraction()
{
    if (Input.GetKeyDown(KeyCode.Return))
    {
        if (!allowMovement && !awaitingWASDInput)
        {
            // First time Enter is pressed
            textBox.text = "You can move around using the WASD keys. Go ahead.";
            StartCoroutine(AllowMovementForSeconds(3.5f));
            awaitingWASDInput = true;
        }
        else if (awaitingWASDInput)
        {
            // Clear text and allow movement indefinitely after pressing Enter again
            textBox.text = "";
            allowMovement = true;
            awaitingWASDInput = false; // Reset to not waiting for WASD input
        }
    }
}

IEnumerator AllowMovementForSeconds(float seconds)
{
    allowMovement = true; // Enable movement
    yield return new WaitForSeconds(seconds); // Wait for specified seconds

    // After 3 seconds, stop movement and prompt to press enter
    allowMovement = false;
    textBox.text = "You can move around using the WASD keys. Go ahead. please Press enter to continue."; // Update text to prompt Enter press
    awaitingWASDInput = true; // Ensure we're waiting for Enter to be pressed before allowing movement again
}


    void AnimateMovementSprite()
    {
        spriteTimer += Time.deltaTime;
        if (spriteTimer > changeSpriteInterval)
        {
            if (currentSpriteIndex == 2)
            {
                spriteRenderer.sprite = sprite2;
                currentSpriteIndex = 3;
            }
            else
            {
                spriteRenderer.sprite = sprite3;
                currentSpriteIndex = 2;
            }
            spriteTimer = 0f; // Reset timer
        }
    }

    
}
