using UnityEngine;
using TMPro;

public class TreeManager : MonoBehaviour
{
    public int[] numbersToInsert;
    private int currentIndex = 0;

    public TMP_Text currentNumberText;
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


    private void Start()
    {
        UpdateNumberDisplay();
        allNodes = FindObjectsByType<TreeNode>(FindObjectsSortMode.None);
    }

    private void Update()
{
    if (puzzleFinished)
        return; 

    if (Input.GetKeyDown(KeyCode.R))
    {
        if (puzzleStarted)
        {
            ResetPuzzle();
        }
        else
        {
            ResetPlayerToLevelStart(); 
        }
    }
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

    puzzleStarted = true;

    if (puzzleWall != null)
        puzzleWall.SetActive(true);
}

    public void HideNextNumber()
    {
        if (currentNumberText != null)
            currentNumberText.gameObject.SetActive(false);
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

    // Instantly reset camera positions too
    if (zoomedOutCamera != null && player != null)
    {
        Vector3 camPos = zoomedOutCamera.transform.position;
        camPos.y = player.position.y;
        zoomedOutCamera.transform.position = camPos;
    }

    if (normalCamera != null && player != null)
    {
        Vector3 camPos = normalCamera.transform.position;
        camPos.x = player.position.x;
        camPos.y = player.position.y;
        normalCamera.transform.position = camPos;
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
    if (player != null && levelResetPoint != null)
    {
        player.position = levelResetPoint.position;
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    if (normalCamera != null && player != null)
    {
        Vector3 camPos = normalCamera.transform.position;
        camPos.x = player.position.x;
        camPos.y = player.position.y;
        normalCamera.transform.position = camPos;
    }
}
}
