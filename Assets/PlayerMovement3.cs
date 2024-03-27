using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement3 : MonoBehaviour
{
    public TextMeshProUGUI hintText; // Assign in Unity Editor
    public float speed = 5f; // Assign in Unity Editor
    public Sprite idleSprite; // Assign in Unity Editor
    public Sprite movingSprite1; // Assign in Unity Editor
    public Sprite movingSprite2; // Assign in Unity Editor
    public Sprite controlSprite; // Assign in Unity Editor
    public Sprite swordSprite; // Assign in Unity Editor

    private float spriteTimer = 0f; // Keep track of time since last sprite change
    private float changeSpriteInterval = 0.1f; // Interval to change sprite, adjust as needed
    private int currentSpriteIndex = 0; // Keep track of the current sprite

    public static int controlNum = 0;

    private bool canMove = false;
    private bool isSwitchingToSword = false;
    private bool isMoving = false;
    private bool isControlActive = false; // Assuming you have logic for control mode elsewhere
    private bool hasControlBeenPressed = false; // To check if control has been pressed at least once
    private float inactivityTimer = 0f;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        hintText.text = "Press Space to Use your knife to cut trees and beasts in both forms. Go ahead and use this knowledge to push the crate to the hole in the wall to walk across.";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            canMove = true;
            hintText.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && !isControlActive)
        {
            StartCoroutine(ControlSpriteCoroutine());
            hasControlBeenPressed = true; // Control has been pressed
        }

        if (!canMove || isControlActive || isSwitchingToSword)
            return;

        HandleMovementInput();

        // Handle Sword Sprite Switch
        if (Input.GetKeyDown(KeyCode.Space) && canMove && !hasControlBeenPressed && !isSwitchingToSword)
        {
            StartCoroutine(SwitchToSwordSprite());
        }
    }

    private void HandleMovementInput()
    {
        Vector3 moveDirection = Vector3.zero;
        isMoving = false; // Reset moving flag at the beginning of each Update call

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
            transform.position += moveDirection.normalized * speed * Time.deltaTime;
            AnimateMovementSprite();
            inactivityTimer = 0f; // Reset inactivity timer when moving
        }
        else if (!isControlActive)
        {
            // Handle idle sprite switch
            inactivityTimer += Time.deltaTime;
            if (inactivityTimer >= 0f)
            {
                spriteRenderer.sprite = idleSprite;
                inactivityTimer = 0f;
            }
        }
    }

    private void AnimateMovementSprite()
    {
        spriteTimer += Time.deltaTime;
        if (spriteTimer > changeSpriteInterval)
        {
            Sprite[] movementSprites = { movingSprite1, movingSprite2 };
            currentSpriteIndex = (currentSpriteIndex + 1) % movementSprites.Length;
            spriteRenderer.sprite = movementSprites[currentSpriteIndex];
            spriteTimer = 0f; // Reset timer
        }
    }

    IEnumerator ControlSpriteCoroutine()
    {
    isControlActive = true;
    if (controlNum == 0) {
    canMove = false; // Ensure player cannot move while control sprite is active
    spriteRenderer.sprite = controlSprite;

    // Perform the shake effect
    float endTime = Time.time + 1.5f;
    while (Time.time < endTime)
    {
        transform.position += new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0);
        yield return null; // Wait for the next frame
    }

    // Reset to idle sprite and allow for re-initiation
    spriteRenderer.sprite = idleSprite;
    isControlActive = false;
    controlNum = 1;
    }
    else if (controlNum == 1) {
        canMove = false; // Ensure player cannot move while control sprite is active
    spriteRenderer.sprite = controlSprite;

    // Perform the shake effect
    float endTime = Time.time + 1.5f;
    while (Time.time < endTime)
    {
        transform.position += new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0);
        yield return null; // Wait for the next frame
    }

    // Reset to idle sprite and allow for re-initiation
    spriteRenderer.sprite = idleSprite;
    hasControlBeenPressed = false;
    isControlActive = false;
    canMove = true;
    controlNum = 0;
    }
    }
    IEnumerator SwitchToSwordSprite()
    {
        isSwitchingToSword = true; // Disable movement and idle sprite logic
        spriteRenderer.sprite = swordSprite;

        // Disable the player's ability to move while the sword sprite is active
        bool originalCanMove = canMove;
        canMove = false;

        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds

        // Re-enable movement and reset to idle sprite
        canMove = originalCanMove;
        spriteRenderer.sprite = idleSprite;
        isSwitchingToSword = false; // Re-enable movement and idle sprite logic
    }
}