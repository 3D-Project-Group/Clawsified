using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColorPuzzleController : MonoBehaviour
{
    public enum ColorPuzzle { Yellow = 0, Green = 1, Blue = 2 }

    public ColorPuzzle[] correctAnswer;
    public ColorPuzzle[] currentValues;

    public bool isActive = true;
    public bool randomStart = true;
    [SerializeField] private GameObject[] gameObjectsToUnactivate;

    private void Awake()
    {
        if (isActive && randomStart)
        {
            currentValues = new ColorPuzzle[correctAnswer.Length];
            for(int i = 0; i < currentValues.Length; i++)
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
}
