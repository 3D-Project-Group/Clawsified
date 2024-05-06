using UnityEngine;

public class MemoryPuzzleInteract : Interact
{
    [Header("Memory Puzzle")]
    [SerializeField] private GameObject memoryPuzzle;
    [SerializeField] private GameObject[] UIToHide;
    [SerializeField] private GameObject[] objsToUnactivate;
    public override void Interaction()
    {
        base.Interaction();

        foreach (GameObject obj in UIToHide)
        {
            obj.SetActive(false);
        }

        GameObject.FindWithTag("Player").GetComponent<PlayerController>().doingPuzzle = true;

        memoryPuzzle.SetActive(true);
        memoryPuzzle.GetComponent<MemoryPuzzleController>().objectsToUnactivate = objsToUnactivate;
        memoryPuzzle.GetComponent<MemoryPuzzleController>().StartPuzzle();
    }
}
