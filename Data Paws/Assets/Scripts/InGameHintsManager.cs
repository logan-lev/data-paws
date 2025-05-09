using UnityEngine;

public class InGameHintsManager : MonoBehaviour
{
    public GameObject bgPanel;
    private bool isPaused = false;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            bgPanel.SetActive(!bgPanel.activeSelf);
        }
    }
}
