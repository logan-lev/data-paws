using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public Transform[] boxes;
    public Transform[] boxStartPositions;
    public BoxHole[] holes;
    public GameObject door; // the wall that disappears when correct

    public Transform player;
    private Vector3 playerStartPos;

    private void Start()
    {
        playerStartPos = player.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPuzzle();
        }

        if (IsPuzzleSolved())
        {
            if (door != null)
                door.SetActive(false); // open the door
        }
    }

    void ResetPuzzle()
    {
        for (int i = 0; i < boxes.Length; i++)
        {
            boxes[i].position = boxStartPositions[i].position;
            Rigidbody2D rb = boxes[i].GetComponent<Rigidbody2D>();
            if (rb) rb.linearVelocity = Vector2.zero;
        }

        player.position = playerStartPos;
        Rigidbody2D prb = player.GetComponent<Rigidbody2D>();
        if (prb) prb.linearVelocity = Vector2.zero;
    }

    bool IsPuzzleSolved()
    {
        foreach (BoxHole hole in holes)
        {
            if (!hole.isCorrect)
                return false;
        }
        return true;
    }
}

