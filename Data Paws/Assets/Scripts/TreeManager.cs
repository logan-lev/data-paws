using UnityEngine;
using TMPro;

public class TreeManager : MonoBehaviour
{
    public int[] numbersToInsert;
    private int currentIndex = 0;

    public TMP_Text currentNumberText;
    public GameObject numberTextBackground;

    public Door door;

    [Header("Camera Switching")]
    public Camera normalCamera;
    public Camera zoomedOutCamera;

    [Header("Reset Settings")]
    public Transform player;
    public Transform playerResetPoint;

    private TreeNode[] allNodes;

    private bool puzzleStarted = false;
    public GameObject puzzleWall;
    public Transform levelResetPoint;
    public Transform puzzleResetPoint;
    private bool puzzleFinished = false;

    [Header("UI")]
    public GameObject pickupPrompt;
    public GameObject pickupBackground;

    [Header("Checkpoint System")]
    public Transform[] checkpointTriggers;
    private Vector3 currentCheckpointPosition;




    void Start()
    {
        UpdateNumberDisplay();
        allNodes = FindObjectsByType<TreeNode>(FindObjectsSortMode.None);

        // Set initial checkpoint to levelResetPoint
        if (levelResetPoint != null)
        {
            currentCheckpointPosition = levelResetPoint.position;
        }
    }


    private void Update()
    {
        if (puzzleFinished)
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (puzzleStarted)
                ResetPuzzle();
            else
                ResetPlayerToLevelStart();
        }

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

        if (pickupPrompt != null)
            pickupPrompt.SetActive(nearInteractable);

        if (pickupBackground != null)
            pickupBackground.SetActive(nearInteractable);
    } 

    public int GetCurrentNumber()
    {
        return numbersToInsert[currentIndex];
    }

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


    public void HideNextNumber()
{
    if (currentNumberText != null)
        currentNumberText.gameObject.SetActive(false);

    if (numberTextBackground != null)
        numberTextBackground.SetActive(false);
}



    private void UpdateNumberDisplay()
    {
        if (currentIndex < numbersToInsert.Length)
            currentNumberText.text = numbersToInsert[currentIndex].ToString();
        else
            currentNumberText.text = "";
    }

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

    public void ResetPuzzle()
{
    // Reset player position
    if (player != null && puzzleResetPoint != null)
    {
        player.position = puzzleResetPoint.position;
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }


    // Reset all nodes
    foreach (var node in allNodes)
    {
        node.ResetNode();
    }

    // Reset puzzle sequence
    currentIndex = 0;
    UpdateNumberDisplay();
    ShowNextNumber();

    // Make sure zoomed-out camera is active again
    if (zoomedOutCamera != null) zoomedOutCamera.enabled = true;
    if (normalCamera != null) normalCamera.enabled = false;
}

    private void ResetPlayerToLevelStart()
{
    RespawnAtCheckpoint();
}

    public void UpdateCheckpoint(Vector3 newCheckpoint)
    {
        currentCheckpointPosition = newCheckpoint;
    }
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
public void HideInteractUI()
{
    if (pickupPrompt != null)
        pickupPrompt.SetActive(false);

    if (pickupBackground != null)
        pickupBackground.SetActive(false);
}



}
