using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs;
    public Transform spawnPoint;
    public PuzzleManagerlvl2 puzzleManager;

    private int currentIndex = 0;
    private GameObject currentItem;

    public void SpawnNext()
    {
        if (currentIndex >= itemPrefabs.Length) return;

        currentItem = Instantiate(itemPrefabs[currentIndex], spawnPoint.position, Quaternion.identity);
        PickupItem pickup = currentItem.GetComponent<PickupItem>();

        if (pickup != null && puzzleManager != null)
            puzzleManager.UpdateCurrentItemName(pickup.itemName);

        currentIndex++;
    }

    public void ResetItems()
    {
        currentIndex = 0;

        if (currentItem != null)
            Destroy(currentItem);

        currentItem = null;

        SpawnNext();
    }

    public bool HasMoreItems()
    {
        return currentIndex < itemPrefabs.Length;
    }
}