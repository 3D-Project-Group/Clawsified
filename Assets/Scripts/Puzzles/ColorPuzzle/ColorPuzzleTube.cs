using System;
using System.Linq;
using UnityEngine;
using static ColorPuzzleController;

public class ColorPuzzleTube : MonoBehaviour
{
    public ColorPuzzleController controller;

    [SerializeField] private int objPosition = 0;
    [SerializeField] private Material[] materials;

    public ColorPuzzle currentColor;
    void Start()
    {
        controller = transform.parent.GetComponent<ColorPuzzleController>();
        if (controller.isActive)
        {
            currentColor = controller.currentValues[objPosition];
            this.gameObject.GetComponent<MeshRenderer>().material = materials[(int)currentColor];
        }
    }

    public void SetValue(ColorPuzzle value)
    {
        currentColor = value;
        controller.currentValues[objPosition] = value;
        this.gameObject.GetComponent<MeshRenderer>().material = materials[(int)value];

        if (controller.currentValues.SequenceEqual(controller.correctAnswer))
        {
            controller.UnactiveObjects();
        }
    }
}
