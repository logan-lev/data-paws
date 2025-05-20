using UnityEngine;
using TMPro;

// Manages the logic for a tree-based number insertion puzzle
public class TreeManager : MonoBehaviour
{
    // Array of numbers to insert into the tree
    public int[] numbersToInsert;
    private int currentIndex = 0;

    // UI elements to display the current number
    public TMP_Text currentNumberText;
    public GameObject numberTextBackground;

    // Reference to the door to open on puzzle completion
    public Door door;

    [Header("Camera Switching")]
    // Cameras used for normal and zoomed-out views
    public Camera normalCamera;
    public Camera zoomedOutCamera;

    [Header("Reset Settings")]
    // Player and reset point references
    public Transform player;
    public Transform playerResetPoint;

    // Array to store all tree nodes in the scene
    private TreeNode[] allNodes;

    // Puzzle state flags
    private bool puzzleStarted = false;
    public GameObject puzzleWall;
    public Transform levelResetPoint;
    public Transform puzzleResetPoint;
    private bool puzzleFinished = false;

    [Header("UI")]
    // UI elements for pickup/interact prompts
    public GameObject pickupPrompt;
    public GameObject pickupBackground;

    [Header("Checkpoint System")]
    // Optional trigger positions and current checkpoint
    public Transform[] checkpointTriggers;
    private Vector3 currentCheckpointPosition;

    void Start()
    {
        // Set up initial number and find all nodes in the scene
        UpdateNumberDisplay();
        allNodes = FindObjectsByType<TreeNode>(FindObjectsSortMode.None);

        // Set initial checkpoint position
        if (levelResetPoint != null)
        {
            currentCheckpointPosition = levelResetPoint.position;
        }
    }

    // Called every frame to monitor player input and proximity to nodes
    private void Update()
    {
        // Stop if the puzzle has already been completed
        if (puzzleFinished)
            return;

        // Handle reset input
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (puzzleStarted)
                ResetPuzzle();
            else
                ResetPlayerToLevelStart();
        }

        // Check for interactable tree nodes nearby
        bool nearInteractable = false;
        Collider2D[] hits = Physics2D.OverlapCircleAll(player.position, 1.5f);

        foreach (var hit in hits)
        {
            TreeNode node = hit.GetComponent<TreeNode>();
            if (node != null && !node.isFilled && !node.alwaysFilled)
            {
                nearInteractable = true;
                break;
            }
        }

        // Toggle UI based on proximity
        if (pickupPrompt != null)
            pickupPrompt.SetActive(nearInteractable);

        if (pickupBackground != null)
            pickupBackground.SetActive(nearInteractable);
    }

    // Returns the current number to be inserted
    public int GetCurrentNumber()
    {
        return numbersToInsert[currentIndex];
    }

    // Advances to the next number, or completes the puzzle
    public void NextNumber()
    {
        currentIndex++;
        if (currentIndex >= numbersToInsert.Length)
        {
            TreeCompleted();
        }
        else
        {
            UpdateNumberDisplay();
        }
    }

    // Shows the number and background UI, starts the puzzle
    public void ShowNextNumber()
    {
        if (currentNumberText != null)
            currentNumberText.gameObject.SetActive(true);

        if (numberTextBackground != null)
            numberTextBackground.SetActive(true);

        puzzleStarted = true;

        if (puzzleWall != null)
            puzzleWall.SetActive(true);
    }

    // Hides the number and background UI
    public void HideNextNumber()
    {
        if (currentNumberText != null)
            currentNumberText.gameObject.SetActive(false);

        if (numberTextBackground != null)
            numberTextBackground.SetActive(false);
    }

    // Updates the displayed number in the UI
    private void UpdateNumberDisplay()
    {
        if (currentIndex < numbersToInsert.Length)
            currentNumberText.text = numbersToInsert[currentIndex].ToString();
        else
            currentNumberText.text = "";
    }

    // Called when the tree puzzle is completed
    private void TreeCompleted()
    {
        door.Open();

        if (zoomedOutCamera != null)
            zoomedOutCamera.enabled = false;

        if (normalCamera != null)
            normalCamera.enabled = true;

        HideNextNumber();
        HideInteractUI();

        puzzleStarted = false;
        puzzleFinished = true;
    }

    // Resets the puzzle to its initial state
    public void ResetPuzzle()
    {
        // Move player to puzzle reset point
        if (player != null && puzzleResetPoint != null)
        {
            player.position = puzzleResetPoint.position;
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }

        // Reset all tree nodes
        foreach (var node in allNodes)
        {
            node.ResetNode();
        }

        // Reset number sequence
        currentIndex = 0;
        UpdateNumberDisplay();
        ShowNextNumber();

        // Activate zoomed-out view again
        if (zoomedOutCamera != null) zoomedOutCamera.enabled = true;
        if (normalCamera != null) normalCamera.enabled = false;
    }

    // Sends the player back to the current checkpoint
    private void ResetPlayerToLevelStart()
    {
        RespawnAtCheckpoint();
    }

    // Updates the player's current checkpoint position
    public void UpdateCheckpoint(Vector3 newCheckpoint)
    {
        currentCheckpointPosition = newCheckpoint;
    }

    // Moves the player to the last checkpoint position
    public void RespawnAtCheckpoint()
    {
        if (player != null)
        {
            player.position = new Vector3(currentCheckpointPosition.x, currentCheckpointPosition.y, 0f);
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }
    }

    // Hides the interact/pickup prompt UI
    public void HideInteractUI()
    {
        if (pickupPrompt != null)
            pickupPrompt.SetActive(false);

        if (pickupBackground != null)
            pickupBackground.SetActive(false);
    }
}