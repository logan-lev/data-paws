using UnityEngine;
using UnityEngine.UI;

public class NPCBehavior : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject npcDialogue;
    public GameObject npcSample;
    public Rigidbody2D npcBody;
    public GameObject player;
    public Rigidbody2D playerBody;
    void Start()
    {
        npcDialogue.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D npcBody)
    {
        npcDialogue.SetActive(true);
    }
}
