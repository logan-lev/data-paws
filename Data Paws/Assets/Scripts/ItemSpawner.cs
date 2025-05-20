using UnityEngine;

// This script handles spawning a sequence of items for a puzzle,
// keeping track of the current item and notifying the puzzle manager.
public class ItemSpawner : MonoBehaviour
{
    // Array of item prefabs to be spawned in order
    public GameObject[] itemPrefabs;

    // Location where items should spawn
    public Transform spawnPoint;

    // Reference to the puzzle manager to update item info
    public PuzzleManagerlvl2 puzzleManager;

    // Index of the current item in the array
    private int currentIndex = 0;

    // Reference to the currently spawned item
    private GameObject currentItem;

    // Spawns the next item in the sequence
    public void SpawnNext()
    {
        // Do nothing if we've reached the end of the array
        if (currentIndex >= itemPrefabs.Length) return;

        // Instantiate the current item at the spawn point
        currentItem = Instantiate(itemPrefabs[currentIndex], spawnPoint.position, Quaternion.identity);

        // Try to get the PickupItem component from the new item
        PickupItem pickup = currentItem.GetComponent<PickupItem>();

        // Notify the puzzle manager of the new item's name
        if (pickup != null && puzzleManager != null)
            puzzleManager.UpdateCurrentItemName(pickup.itemName);

        // Move to the next item index
        currentIndex++;
    }

    // Resets the item sequence and spawns the first item again
    public void ResetItems()
    {
        // Reset index
        currentIndex = 0;

        // Destroy the currently active item if it exists
        if (currentItem != null)
            Destroy(currentItem);

        // Clear the current item reference
        currentItem = null;

        // Spawn the first item
        SpawnNext();
    }

    // Checks whether there are more items to spawn
    public bool HasMoreItems()
    {
        return currentIndex < itemPrefabs.Length;
    }
}