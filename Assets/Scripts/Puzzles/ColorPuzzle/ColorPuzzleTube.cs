using System;
using System.Linq;
using UnityEngine;
using static ColorPuzzleController;

public class ColorPuzzleTube : MonoBehaviour
{
    public ColorPuzzleController controller;
    [SerializeField] private MeshRenderer tubeLiquidMeshRenderer;

    [SerializeField] private int objPosition = 0;
    [SerializeField] private Material[] materials;

    public ColorPuzzle currentColor;
    void Start()
    {
        controller = transform.parent.GetComponent<ColorPuzzleController>();
        if (controller.isActive)
        {
            currentColor = controller.currentValues[objPosition];
            tubeLiquidMeshRenderer.material = materials[(int)currentColor];
        }
    }

    public void SetValue(ColorPuzzle value)
    {
        currentColor = value;
        controller.currentValues[objPosition] = value;
        tubeLiquidMeshRenderer.material = materials[(int)value];

        if (controller.currentValues.SequenceEqual(controller.correctAnswer))
        {
            controller.UnactiveObjects();
        }
    }
}
