using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainPlayerMovement : MonoBehaviour
{
    public enum MapType { Map1, Map2, Map3, AllMaps }
    public MapType currentMap;

    public TextMeshProUGUI hintText;
    public Sprite idleSprite, movingSprite1, movingSprite2, controlSprite, swordSprite;
    public AudioClip movementSound;
    public AudioClip controlSound;

    public float speed = 5f;
    private float spriteTimer = 0f;
    private float changeSpriteInterval = 0.1f;
    private int currentSpriteIndex = 0;
    private bool canMove = false;
    private bool isMoving = false;
    private bool isControlActive = false;
    private bool enterPressed = false;
    private bool isSwitchingToSword = false;
    private bool awaitingWASDInput = false;
    private bool firstWASDPressed = false;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource; // AudioSource component

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
    }

    private void Start()
    {
        SetInitialHintText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            enterPressed = true;
            HandleEnterPress();
        }

        if (canMove && !firstWASDPressed && currentMap == MapType.Map1)
        {
            CheckForInitialMovementInput();
        }

        if (currentMap != MapType.Map1 && ((currentMap == MapType.Map2 || currentMap == MapType.Map3) && enterPressed) || currentMap == MapType.AllMaps)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                StartCoroutine(ControlSpriteCoroutine());
            }
        }

        if (!canMove || isControlActive || isSwitchingToSword)
            return;

        HandleMovementInput();

        if ((currentMap == MapType.Map3 || currentMap == MapType.AllMaps) && !isControlActive && !isSwitchingToSword)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(SwitchToSwordSprite());
            }
        }
    }

    private void SetInitialHintText()
    {
        switch (currentMap)
        {
            case MapType.Map1:
                hintText.text = "Hi, it is I, your astral self. Press Enter to begin.";
                break;
            case MapType.Map2:
            case MapType.Map3:
                hintText.text = "By pressing control you can switch between me and your body, so I may help you in need. Press Enter";
                break;
            case MapType.AllMaps:
                hintText.text = "Press Space to Use your knife to cut trees and beasts in both forms. Go ahead and use this knowledge to push the crate to the hole in the wall to walk across.";
                hintText.gameObject.SetActive(false);  // Hide text as no Enter key action is needed to start
                canMove = true;  // Enable movement immediately
                break;
        }
    }

    private void HandleEnterPress()
{
    if (currentMap == MapType.Map1 && !awaitingWASDInput)
    {
        // Only update text and set canMove to true if it's the initial press
        hintText.text = "You can move around using the WASD keys. Go ahead and try.";
        canMove = true; // Allow movement, but timer starts on WASD press
    }
    else
    {
        // Final phase after second Enter press
        canMove = true;
        hintText.gameObject.SetActive(false); // Optionally hide the text
    }
}

    private void HandleMovementInput()
    {
        Vector3 moveDirection = Vector3.zero;
        bool wasMoving = isMoving;
        isMoving = false;

        if (Input.GetKey(KeyCode.D)) { moveDirection += Vector3.right; isMoving = true; }
        if (Input.GetKey(KeyCode.A)) { moveDirection += Vector3.left; isMoving = true; }
        if (Input.GetKey(KeyCode.W)) { moveDirection += Vector3.up; isMoving = true; }
        if (Input.GetKey(KeyCode.S)) { moveDirection += Vector3.down; isMoving = true; }

        if (isMoving)
        {
            if (!wasMoving) // Start the sound when movement starts
                audioSource.Play();

            transform.position += moveDirection.normalized * speed * Time.deltaTime;
            AnimateMovementSprite();
        }
        else
        {
            if (wasMoving) // Stop the sound when movement stops
                audioSource.Stop();

            if (!isControlActive)
                spriteRenderer.sprite = idleSprite;
        }
    }

    private void CheckForInitialMovementInput()
{
    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
    {
        if (!firstWASDPressed)
        {
            firstWASDPressed = true; // Ensures the 5-second timer starts only once
            StartCoroutine(AllowMovementForSeconds(2)); // Start the 5-second timer
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
            spriteTimer = 0f;
        }
    }

    IEnumerator AllowMovementForSeconds(float seconds)
{
    yield return new WaitForSeconds(seconds);
    if (firstWASDPressed) // Make sure timer affects movement only after first WASD press
    {
        canMove = false; // Disable movement after 5 seconds
        awaitingWASDInput = true;
        hintText.text = "You can move around using the WASD keys. Go ahead and try. Press Enter to continue."; // Set flag to show new text and wait for Enter press
    }
}

    IEnumerator ControlSpriteCoroutine()
{
    // Toggle control mode
    if (!isControlActive)
    {
        isControlActive = true; // Enable control mode for initial press
        spriteRenderer.sprite = controlSprite;
        audioSource.clip = controlSound;
        audioSource.Play();
        float endTime = Time.time + 1.5f;
        while (Time.time < endTime)
        {
            transform.position += new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0);
            yield return null; // Wait for the next frame
        }
        canMove = false; // Disable WASD movement while in control mode
        spriteRenderer.sprite = idleSprite;
        audioSource.Stop();
    }
    else
    {
        isControlActive = false; // Disable control mode for second press
        spriteRenderer.sprite = controlSprite;
        audioSource.clip = controlSound;
        audioSource.Play();
        float endTime = Time.time + 1.5f;
        while (Time.time < endTime)
        {
            transform.position += new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0);
            yield return null; // Wait for the next frame
        }
        canMove = true;
        spriteRenderer.sprite = idleSprite;
        audioSource.Stop();
    }
}


    IEnumerator SwitchToSwordSprite()
    {
        isSwitchingToSword = true;
        spriteRenderer.sprite = swordSprite;
        canMove = false;

        yield return new WaitForSeconds(0.5f);

        canMove = true;
        spriteRenderer.sprite = idleSprite;
        isSwitchingToSword = false;
    }
}
