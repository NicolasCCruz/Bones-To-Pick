using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement2 : MonoBehaviour
{
    public TextMeshProUGUI hintText; // Assign in Unity Editor
    public float speed = 5f; // Assign in Unity Editor
    public Sprite idleSprite; // Assign in Unity Editor
    public Sprite movingSprite1; // Assign in Unity Editor
    public Sprite movingSprite2; // Assign in Unity Editor
    public Sprite controlSprite; // Assign in Unity Editor

    private float spriteTimer = 0f; // Keep track of time since last sprite change
private float changeSpriteInterval = 0.1f; // Interval to change sprite, adjust as needed
private int currentSpriteIndex = 0; // Keep track of the current sprite

    public static int controlNum = 0;

    private bool canMove = false;
    private bool isMoving = false;
    private bool isControlActive = false; // Assuming you have logic for control mode elsewhere
    private float inactivityTimer = 0f;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        hintText.text = "You can summon me for help by pressing control. Press Enter";
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

        }

        if (!canMove || isControlActive)
            return;

        Vector3 moveDirection = Vector3.zero;
        isMoving = false; // Reset moving flag at the beginning of each Update call

        // Input handling for movement
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
            // Increase inactivity timer if not moving and not in control mode
            inactivityTimer += Time.deltaTime;
            if (inactivityTimer >= 0.5f)
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
        // Example: Assuming you have an array of sprites for movement
        Sprite[] movementSprites = new Sprite[] {movingSprite1, movingSprite2}; // Add your sprites here

        // Cycle through the movement sprites
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
    float endTime = Time.time + 3.5f;
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
    float endTime = Time.time + 3.5f;
    while (Time.time < endTime)
    {
        transform.position += new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0);
        yield return null; // Wait for the next frame
    }

    // Reset to idle sprite and allow for re-initiation
    spriteRenderer.sprite = idleSprite;
    isControlActive = false;
    canMove = true;
    controlNum = 0;
    }
    }
}