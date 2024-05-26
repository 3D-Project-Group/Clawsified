using UnityEngine;

public class MemoryPuzzleInteract : Interact
{
    [Header("Memory Puzzle")]
    [SerializeField] private GameObject memoryPuzzle;
    [SerializeField] private GameObject[] UIToHide;
    [SerializeField] private GameObject[] objsToUnactivate;
    public bool active = true;
    public override void Interaction()
    {
        if (active)
        {
            base.Interaction();

            foreach (GameObject obj in UIToHide)
            {
                obj.SetActive(false);
            }

            GameObject.FindWithTag("Player").GetComponent<PlayerController>().doingPuzzle = true;

            memoryPuzzle.SetActive(true);
            MemoryPuzzleController puzzleController = memoryPuzzle.GetComponent<MemoryPuzzleController>();
            puzzleController.objectsToUnactivate = objsToUnactivate;
            puzzleController.UIToShow = UIToHide;
            puzzleController.puzzleToDeactivate = this;
            puzzleController.StartPuzzle();
        }
    }
}
