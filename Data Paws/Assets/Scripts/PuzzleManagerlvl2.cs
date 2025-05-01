using UnityEngine;
using TMPro;

public class PuzzleManagerlvl2 : MonoBehaviour
{
    public ItemSpawner itemSpawner;
    public Door2 door;

    [Header("UI Elements")]
    public TMP_Text currentItemText;

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

    private void Start()
    {
        if (itemSpawner != null)
            totalItems = itemSpawner.itemPrefabs.Length;

        if (currentItemText != null)
        {
            currentItemText.text = "Item: —";
            currentItemText.gameObject.SetActive(false);
        }

        if (zoomedOutCamera != null)
            zoomedOutCamera.enabled = false;

        if (normalCamera != null)
            normalCamera.enabled = true;
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

            if (invisibleWall != null)
                invisibleWall.SetActive(false);
        }
    }

    public void UpdateCurrentItemName(string name)
    {
        if (currentItemText != null)
            currentItemText.text = "Item: " + name;
    }

    private void Update()
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

    private void ResetPlayerToLevelStart()
    {
        if (player != null && levelResetPoint != null)
        {
            player.position = levelResetPoint.position;
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = Vector2.zero;
        }

        if (normalCamera != null)
        {
            normalCamera.transform.position = new Vector3(
                player.position.x,
                player.position.y,
                normalCamera.transform.position.z
            );
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
}