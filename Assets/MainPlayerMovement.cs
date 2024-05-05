using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainPlayerMovement : MonoBehaviour
{
    public enum MapType { Map1, Map2, Map3 }
    public MapType currentMap;

    // Text and sprites assignable in Unity Editor
    public TextMeshProUGUI hintText;
    public Sprite idleSprite, movingSprite1, movingSprite2, controlSprite, swordSprite;

    // Movement and animation settings
    public float speed = 5f;
    private float spriteTimer = 0f;
    private float changeSpriteInterval = 0.1f;
    private int currentSpriteIndex = 0;
    private float inactivityTimer = 0f;

    private bool canMove = false;
    private bool isMoving = false;
    private bool isControlActive = false;
    private bool hasControlBeenPressed = false;
    private bool isSwitchingToSword = false;
    private bool awaitingWASDInput = false; // Specific to Map1


    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        switch (currentMap)
        {
            case MapType.Map1:
                hintText.text = "Hi it is I, your astral self. I am here to assist you through your journey. Press enter";
                break;
            case MapType.Map2:
                hintText.text = "By pressing control you can switch between me and your body, so I may help you in need. Press Enter";
                break;
            case MapType.Map3:
                hintText.text = "Press Space to Use your knife to cut trees and beasts in both forms. Go ahead and use this knowledge to push the crate to the hole in the wall to walk across.";
                break;
        }
    }

    private void Update()
{
    if (Input.GetKeyDown(KeyCode.Return))
    {
        if (currentMap == MapType.Map1 && !canMove && !awaitingWASDInput)
        {
            hintText.text = "You can move around using the WASD keys. Go ahead.";
            StartCoroutine(AllowMovementForSeconds(3.5f));
            awaitingWASDInput = true;
        }
        else if (currentMap == MapType.Map1 && awaitingWASDInput)
        {
            hintText.text = "";
            canMove = true;
            awaitingWASDInput = false;
        }
        else
        {
            canMove = true;
            hintText.gameObject.SetActive(false);
        }
    }

    if (currentMap == MapType.Map2 || currentMap == MapType.Map3)
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCoroutine(ControlSpriteCoroutine());
        }
    }

    // Prevent movement and other interactions if control mode is active or sword switching is in progress
    if (!canMove || isControlActive || isSwitchingToSword)
        return;

    HandleMovementInput();

    // Map3 specific functionality
    if (currentMap == MapType.Map3)
    {
        if (Input.GetKeyDown(KeyCode.Space) && canMove && !hasControlBeenPressed && !isSwitchingToSword)
        {
            StartCoroutine(SwitchToSwordSprite());
        }
    }
}

    private void HandleMovementInput()
    {
        Vector3 moveDirection = Vector3.zero;
        isMoving = false;

        if (Input.GetKey(KeyCode.D)) { moveDirection += Vector3.right; isMoving = true; }
        if (Input.GetKey(KeyCode.A)) { moveDirection += Vector3.left; isMoving = true; }
        if (Input.GetKey(KeyCode.W)) { moveDirection += Vector3.up; isMoving = true; }
        if (Input.GetKey(KeyCode.S)) { moveDirection += Vector3.down; isMoving = true; }

        if (isMoving)
        {
            transform.position += moveDirection.normalized * speed * Time.deltaTime;
            AnimateMovementSprite();
            inactivityTimer = 0f;
        }
        else if (!isControlActive)
        {
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
            Sprite[] movementSprites = { movingSprite1, movingSprite2 };
            currentSpriteIndex = (currentSpriteIndex + 1) % movementSprites.Length;
            spriteRenderer.sprite = movementSprites[currentSpriteIndex];
            spriteTimer = 0f;
        }
    }

    IEnumerator AllowMovementForSeconds(float seconds)
    {
        canMove = true; // Enable movement
        yield return new WaitForSeconds(seconds); // Wait for specified seconds

        // After the interval, prompt to press Enter again to continue
        canMove = false;
        hintText.text = "You can move around using the WASD keys. Go ahead. Press Enter to continue.";
        awaitingWASDInput = true; // Set to waiting for Enter to be pressed before allowing movement again
    }

    IEnumerator ControlSpriteCoroutine()
{
    // Toggle control mode
    if (!isControlActive)
    {
        isControlActive = true; // Enable control mode for initial press
        spriteRenderer.sprite = controlSprite;
        float endTime = Time.time + 1.5f;
    while (Time.time < endTime)
    {
        transform.position += new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0);
        yield return null; // Wait for the next frame
    }
        canMove = false; // Disable WASD movement while in control mode
        spriteRenderer.sprite = idleSprite;
    }
    else
    {
        isControlActive = false; // Disable control mode for second press
        spriteRenderer.sprite = controlSprite;
        float endTime = Time.time + 1.5f;
    while (Time.time < endTime)
    {
        transform.position += new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0);
        yield return null; // Wait for the next frame
    }
        canMove = true;
        spriteRenderer.sprite = idleSprite;
    }

    // Simulate astral movement with control sprite

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