using UnityEngine;

public class ColorPuzzleController : MonoBehaviour
{
    public enum ColorPuzzle { Yellow = 0, Green = 1, Blue = 2 }

    [Header("Puzzle Checking")]
    public ColorPuzzle[] correctAnswer;
    public ColorPuzzle[] currentValues;
    public ColorPuzzleTube[] tubes;
    [Space]
    public bool isActive = true;
    public bool randomStart = true;
    [Space]
    [SerializeField] private GameObject[] gameObjectsToUnactivate;

    private void Awake()
    {
        if (isActive && randomStart)
        {
            currentValues = new ColorPuzzle[correctAnswer.Length];
            for (int i = 0; i < currentValues.Length; i++)
            {
                currentValues[i] = (ColorPuzzle)Random.Range(0, 3);
            }
        }
    }

    public void UnactiveObjects()
    {
        foreach (GameObject obj in gameObjectsToUnactivate)
        {
            obj.SetActive(false);
            isActive = false;
        }
    }

    public void SetTubeColor(int tubeID)
    {
        if(isActive)
        {
            if ((int)tubes[tubeID].currentColor < 2)
            {
                tubes[tubeID].SetValue(tubes[tubeID].currentColor + 1);
            }
            else
            {
                tubes[tubeID].SetValue(0);
            }
        }
    }
}