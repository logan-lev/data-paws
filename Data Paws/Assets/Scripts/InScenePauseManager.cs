using UnityEngine;

// This script manages the in-scene pause system, including toggling pause, resuming, and returning to the main menu.
public class InScenePauseManager : MonoBehaviour
{
    // Reference to the pause menu UI panel
    public GameObject pausePanel;

    // Tracks whether the game is currently paused
    private bool isPaused = false;

    void Update()
    {
        // Press 'P' to toggle the pause menu
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    // Toggles the pause state and UI
    public void TogglePause()
    {
        // Flip the paused state
        isPaused = !isPaused;

        // Show or hide the pause panel based on current pause state
        pausePanel.SetActive(isPaused);

        // Freeze or unfreeze time
        Time.timeScale = isPaused ? 0f : 1f;
    }

    // Resumes the game if it was paused
    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // Switches back to the main menu scene
    public void GoToMainMenu()
    {
        // Unpause the game before transitioning scenes
        Time.timeScale = 1f;

        // Use the SceneTransitionManager to load the main title scene
        SceneTransitionManager.instance.LoadScene("Main Title");
    }
}