using System.Collections;
using UnityEngine;

public class AstralController : MonoBehaviour
{
    public float speed = 5f; // Assign in Unity Editor
    public Sprite idleSprite; // Assign in Unity Editor
    public Sprite movingSprite1; // Assign in Unity Editor
    public Sprite movingSprite2; // Assign in Unity Editor
    public Sprite specialActionSprite; // New sprite for special action

    private float spriteTimer = 0f;
    private float changeSpriteInterval = 0.1f;
    private int currentSpriteIndex = 0;

    private bool canMove = false;
    private bool isMoving = false;
    private bool isFading = false;
    private bool canSpecialAction = false; // New flag to control special action availability
    private bool isPerformingSpecialAction = false; // Flag to track special action

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        spriteRenderer.sprite = idleSprite;
        SetAlpha(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isFading)
        {
            StartCoroutine(FadeSpriteTo(canMove ? 0f : 0.92f));
        }

        if (!canMove)
            return;

        if (Input.GetKeyDown(KeyCode.Space) && canSpecialAction && !isPerformingSpecialAction)
        {
            StartCoroutine(PerformSpecialAction());
        }

        HandleMovement();
    }

    private void HandleMovement()
    {
        if (isPerformingSpecialAction) return;

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
        }
        else
        {
            spriteRenderer.sprite = idleSprite;
        }
    }

    private IEnumerator PerformSpecialAction()
    {
        isPerformingSpecialAction = true;
        spriteRenderer.sprite = specialActionSprite;
        yield return new WaitForSeconds(0.5f); // Display the special sprite for 2 seconds
        isPerformingSpecialAction = false;
    }

    private void AnimateMovementSprite()
    {
        if (isPerformingSpecialAction) return;

        spriteTimer += Time.deltaTime;
        if (spriteTimer > changeSpriteInterval)
        {
            Sprite[] movementSprites = { movingSprite1, movingSprite2 };
            currentSpriteIndex = (currentSpriteIndex + 1) % movementSprites.Length;
            spriteRenderer.sprite = movementSprites[currentSpriteIndex];
            spriteTimer = 0f;
        }
    }

    IEnumerator FadeSpriteTo(float targetAlpha)
    {
        isFading = true;
        float alpha = spriteRenderer.color.a;

        while (!Mathf.Approximately(alpha, targetAlpha))
        {
            alpha = Mathf.MoveTowards(alpha, targetAlpha, Time.deltaTime / 2); // Slowed down the fade speed
            SetAlpha(alpha);
            yield return null;
        }

        isFading = false;
        canMove = alpha > 0;
        canSpecialAction = alpha > 0; // Enable special action when movement is enabled
    }

    private void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}