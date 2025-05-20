using UnityEngine;
using TMPro;

public class PuzzleManagerlvl2 : MonoBehaviour
{
    public ItemSpawner itemSpawner;
    public Door2 door;
    private Vector3 currentCheckpointPosition;

    public PlayerMovement playerMovement;


    [Header("UI Elements")]
    public TMP_Text currentItemText;

    [Header("Pickup Prompt")]
    public GameObject pickupPrompt;
    public GameObject pickupPromptBackground;

    [Header("Cameras")]
    public Camera normalCamera;
    public Camera zoomedOutCamera;

    [Header("Reset System")]
    public Transform levelResetPoint;
    public Transform puzzleResetPoint;
    public Transform player;

    [Header("Puzzle Control")]
    public GameObject invisibleWall;

    private bool puzzleStarted = false;
    private bool puzzleCompleted = false;

    private int itemsSorted = 0;
    private int totalItems;

    private float pickupRange = 2.5f;

    private void Start()
    {
        if (itemSpawner != null)
            totalItems = itemSpawner.itemPrefabs.Length;

        if (currentItemText != null)
        {
            currentItemText.text = "Item: —";
            currentItemText.gameObject.SetActive(false);
        }

        if (pickupPrompt != null)
            pickupPrompt.SetActive(false);
        if (pickupPromptBackground != null)
            pickupPromptBackground.SetActive(false);

        if (zoomedOutCamera != null)
            zoomedOutCamera.enabled = false;

        if (normalCamera != null)
            normalCamera.enabled = true;

        if (levelResetPoint != null)
            currentCheckpointPosition = levelResetPoint.position;

    }

    public void StartPuzzle()
    {
        puzzleStarted = true;
        puzzleCompleted = false;

        if (currentItemText != null)
        {
            currentItemText.text = "Item: —";
            currentItemText.gameObject.SetActive(true);
        }

        if (invisibleWall != null)
            invisibleWall.SetActive(true);

        if (itemSpawner != null)
            itemSpawner.SpawnNext();

        if (zoomedOutCamera != null)
            zoomedOutCamera.enabled = true;

        if (normalCamera != null)
            normalCamera.enabled = false;
    }

    public void OnCorrectPlacement()
    {
        itemsSorted++;

        if (itemSpawner.HasMoreItems())
        {
            itemSpawner.SpawnNext();
        }
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
    public void UpdateCheckpoint(Vector3 newPosition)
{
    currentCheckpointPosition = newPosition;
}

}
