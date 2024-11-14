using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class MainFarmer : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private Vector2 direction;
    [SerializeField]
    private Transform point;
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private FarmController controller;
    private Boolean canClick;
    private int extraLives = 0;
    private Inventory inventory;
    [SerializeField]
    private UI_Inventory uiInventory;

    private Vector3 lastMousePosition;
    private float timeSinceMouseMoved = 0.0f;
    private bool isMouseMoving = false;
    private const float idleMouseTimeout = 3.0f; // Time before switching to movement direction animation
    private Animator animator;

    private void Start()
    {
        canClick = true;
        animator = transform.Find("Body").GetComponent<Animator>();

        // TODO: Implement the inventory logic
        // inventory = new Inventory();
        // uiInventory.SetInventory(inventory);
        // equipmentEffects();

        lastMousePosition = Input.mousePosition;
    }

    public int ExtraLives
    {
        get { return extraLives; }
        set { extraLives = value; } // Ensuring health doesn't go below zero
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(Vector3.forward);
        direction = Vector2.zero;

        // Movement input
        if (Input.GetKey(KeyCode.W)) direction.y = 1;
        if (Input.GetKey(KeyCode.A)) direction.x = -1;
        if (Input.GetKey(KeyCode.S)) direction.y = -1;
        if (Input.GetKey(KeyCode.D)) direction.x = 1;

        direction.Normalize();

        // Debug output to verify direction values
        //Debug.Log("Direction: " + direction);

        // Move the character
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // Call Action when the left mouse button is clicked
        if (canClick && Input.GetMouseButton(0))
        {
            Action();
        }

        // Update animator parameters for movement
        SetAnimationParameters(direction.x, direction.y);
    }

    // TODO: Add equipment effects
    private void equipmentEffects()
    {
        // extraLives++;
    }

    public void Action()
    {
        Vector3Int pos = tilemap.WorldToCell(point.position);
        foreach (Vector3Int tile in controller.farmTiles)
        {
            if (tile.Equals(pos))
            {
                controller.InteractTile(pos);
                StartCoroutine(MouseCoolDown());
                return;
            }
        }
    }

    // Code from Danddx Tutorial on youtube: https://www.youtube.com/watch?v=_XdqA3xbP2A
    void HandleFacingDirection()
    {
        // Check if mouse has moved
        Vector3 mousePosition = Input.mousePosition;
        if (Vector3.Distance(mousePosition, lastMousePosition) > 0.1f) // Mouse moved
        {
            isMouseMoving = true;
            timeSinceMouseMoved = 0.0f;
            lastMousePosition = mousePosition;

            // Face the character towards the mouse
            faceMouse();
        }
        else
        {
            timeSinceMouseMoved += Time.deltaTime;

            // If mouse hasn't moved for idleMouseTimeout seconds, use movement direction for facing
            if (timeSinceMouseMoved > idleMouseTimeout)
            {
                isMouseMoving = false;
                UpdateAnimationBasedOnMovement();
            }
        }

        // Update the animation based on direction if the mouse is moving
        if (isMouseMoving) UpdateAnimationBasedOnMouseDirection();
    }

    void faceMouse()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDirection = (mouseWorldPosition - transform.position).normalized;

        // Set animator parameters based on mouse direction
        animator.SetFloat("LookX", mouseDirection.x);
        animator.SetFloat("LookY", mouseDirection.y);
    }

    // Get the input direction based on the latest key press
    private Vector2 GetInputDirection()
    {
        Vector2 newDirection = Vector2.zero;

        // Check for input in order of priority, allowing the latest key press to take precedence
        if (Input.GetKey(KeyCode.W)) newDirection = Vector2.up;
        if (Input.GetKey(KeyCode.S)) newDirection = Vector2.down;
        if (Input.GetKey(KeyCode.A)) newDirection = Vector2.left;
        if (Input.GetKey(KeyCode.D)) newDirection = Vector2.right;

        return newDirection;
    }

    void UpdateAnimationBasedOnMouseDirection()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDirection = (mouseWorldPosition - transform.position).normalized;

        // Set walking animation based on mouse direction
        SetAnimationParameters(mouseDirection.x, mouseDirection.y);
    }

    void UpdateAnimationBasedOnMovement()
    {
        // Set walking animation based on movement direction
        SetAnimationParameters(direction.x, direction.y);
    }

    void SetAnimationParameters(float x, float y)
    {
        animator.SetFloat("MoveX", x);
        animator.SetFloat("MoveY", y);
        //animator.SetBool("IsWalking", direction.sqrMagnitude > 0); // Update walking state
    }

    IEnumerator MouseCoolDown()
    {
        canClick = false;
        point.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        point.gameObject.SetActive(true);
        canClick = true;
    }
}
