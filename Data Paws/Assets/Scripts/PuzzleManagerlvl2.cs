using UnityEngine;
using TMPro;

// Manages the second level's item-sorting puzzle, including UI, reset logic, and camera control.
public class PuzzleManagerlvl2 : MonoBehaviour
{
    // Reference to the item spawner that generates puzzle items
    public ItemSpawner itemSpawner;

    // Reference to the door that opens when the puzzle is completed
    public Door2 door;

    // Stores the current checkpoint position for resetting the player
    private Vector3 currentCheckpointPosition;

    // Reference to the player's movement script
    public PlayerMovement playerMovement;

    [Header("UI Elements")]
    // Displays the currently held item's name
    public TMP_Text currentItemText;

    [Header("Pickup Prompt")]
    // UI prompts that appear when the player is near an item
    public GameObject pickupPrompt;
    public GameObject pickupPromptBackground;

    [Header("Cameras")]
    // Camera for normal gameplay
    public Camera normalCamera;

    // Camera used for zoomed-out puzzle view
    public Camera zoomedOutCamera;

    [Header("Reset System")]
    // Transform where the player is sent when resetting the full level
    public Transform levelResetPoint;

    // Transform where the player is sent when resetting the puzzle only
    public Transform puzzleResetPoint;

    // Reference to the player's transform
    public Transform player;

    [Header("Puzzle Control")]
    // Invisible wall that activates when the puzzle starts
    public GameObject invisibleWall;

    // Flags to track puzzle state
    private bool puzzleStarted = false;
    private bool puzzleCompleted = false;

    // Number of items successfully sorted and total items to sort
    private int itemsSorted = 0;
    private int totalItems;

    // Distance to check when showing the pickup prompt
    private float pickupRange = 2.5f;

    private void Start()
    {
        // Count total items based on the item spawner's prefabs
        if (itemSpawner != null)
            totalItems = itemSpawner.itemPrefabs.Length;

        // Initialize item text and hide it
        if (currentItemText != null)
        {
            currentItemText.text = "Item: —";
            currentItemText.gameObject.SetActive(false);
        }

        // Hide pickup UI prompts
        if (pickupPrompt != null)
            pickupPrompt.SetActive(false);
        if (pickupPromptBackground != null)
            pickupPromptBackground.SetActive(false);

        // Enable normal camera and disable zoomed-out view
        if (zoomedOutCamera != null)
            zoomedOutCamera.enabled = false;
        if (normalCamera != null)
            normalCamera.enabled = true;

        // Set the starting checkpoint
        if (levelResetPoint != null)
            currentCheckpointPosition = levelResetPoint.position;
    }

    // Called to start the puzzle logic
    public void StartPuzzle()
    {
        puzzleStarted = true;
        puzzleCompleted = false;

        // Reset UI text
        if (currentItemText != null)
        {
            currentItemText.text = "Item: —";
            currentItemText.gameObject.SetActive(true);
        }

        // Activate invisible barrier
        if (invisibleWall != null)
            invisibleWall.SetActive(true);

        // Spawn the first item
        if (itemSpawner != null)
            itemSpawner.SpawnNext();

        // Switch to puzzle camera
        if (zoomedOutCamera != null)
            zoomedOutCamera.enabled = true;
        if (normalCamera != null)
            normalCamera.enabled = false;
    }

    // Called when an item is sorted correctly
    public void OnCorrectPlacement()
    {
        itemsSorted++;

        // If more items remain, spawn the next one
        if (itemSpawner.HasMoreItems())
        {
            itemSpawner.SpawnNext();
        }
        // If all items are sorted, complete the puzzle
        else if (itemsSorted >= totalItems)
        {
            puzzleCompleted = true;
            puzzleStarted = false;

            if (door != null)
            {
                door.Open();
                door.gameObject.SetActive(false);
            }

            if (currentItemText != null)
                currentItemText.gameObject.SetActive(false);

            if (zoomedOutCamera != null)
                zoomedOutCamera.enabled = false;
            if (normalCamera != null)
                normalCamera.enabled = true;
        }
    }

    // Updates the current item name text on UI
    public void UpdateCurrentItemName(string name)
    {
        if (currentItemText != null)
            currentItemText.text = "Item: " + name;
    }

    private void Update()
    {
        HandleReset();
        HandlePickupPrompt();
    }

    // Handles reset logic depending on puzzle state
    private void HandleReset()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (puzzleCompleted)
                return;

            if (puzzleStarted)
                ResetPuzzle();
            else
                ResetPlayerToLevelStart();
        }
    }

    // Shows or hides the pickup prompt based on proximity to an item
    private void HandlePickupPrompt()
    {
        if (playerMovement != null && playerMovement.IsHoldingItem())
        {
            if (pickupPrompt != null)
                pickupPrompt.SetActive(false);
            if (pickupPromptBackground != null)
                pickupPromptBackground.SetActive(false);
            return;
        }

        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        bool showPrompt = false;

        foreach (GameObject item in items)
        {
            float distance = Vector2.Distance(player.position, item.transform.position);
            if (distance <= pickupRange)
            {
                showPrompt = true;
                break;
            }
        }

        if (pickupPrompt != null)
            pickupPrompt.SetActive(showPrompt);
        if (pickupPromptBackground != null)
            pickupPromptBackground.SetActive(showPrompt);
    }

    // Resets the player back to the starting point of the level
    public void ResetPlayerToLevelStart()
    {
        if (player != null)
        {
            player.position = currentCheckpointPosition;
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = Vector2.zero;
        }
    }

    // Fully resets the puzzle: player, items, state, and UI
    private void ResetPuzzle()
    {
        if (player != null && puzzleResetPoint != null)
        {
            player.position = puzzleResetPoint.position;
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = Vector2.zero;
        }

        if (itemSpawner != null)
            itemSpawner.ResetItems();

        itemsSorted = 0;
        puzzleStarted = true;

        if (currentItemText != null)
        {
            currentItemText.gameObject.SetActive(true);
        }

        if (invisibleWall != null)
            invisibleWall.SetActive(true);
    }

    // Updates the current respawn/checkpoint position
    public void UpdateCheckpoint(Vector3 newPosition)
    {
        currentCheckpointPosition = newPosition;
    }
}