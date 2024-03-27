using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateController : MonoBehaviour
{
    public GameObject holeSprite; // Assign in Unity Editor
    public GameObject wallSprite; // Assign in Unity Editor
    public float speed = 2f; // Speed of the movement

    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsWithinRange())
        {
            // Only disable the BoxCollider2D
            GetComponent<BoxCollider2D>().enabled = false;

            MoveTowardsHole();
        }
    }

    bool IsWithinRange()
    {
        if (wallSprite == null) return false;

        float distanceToWall = Vector3.Distance(transform.position, wallSprite.transform.position);
        return distanceToWall <= Mathf.Sqrt(1.6f * 1.6f + 0.5f * 0.5f);
    }

    void MoveTowardsHole()
    {
        if (holeSprite == null) return;

        Vector3 positionToMove = new Vector3(holeSprite.transform.position.x, transform.position.y, transform.position.z);

        // Move on X axis
        if (transform.position.x != holeSprite.transform.position.x)
        {
            transform.position = Vector3.MoveTowards(transform.position, positionToMove, speed * Time.deltaTime);
        }
        // Then, move on Y axis
        else if (transform.position.y != holeSprite.transform.position.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, holeSprite.transform.position, speed * Time.deltaTime);
        }

        // Check if the Crate has arrived at the holeSprite's position
        if (transform.position == holeSprite.transform.position)
        {
            Destroy(holeSprite); // Delete holeSprite object
        }
    }
}
