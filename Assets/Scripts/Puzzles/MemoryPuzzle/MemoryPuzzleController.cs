using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MemoryPuzzleController : MonoBehaviour
{
    
    [Header("Components")]
    [SerializeField] private GameObject[] buttons;
    public MemoryPuzzleInteract puzzleToDeactivate;
    public GameObject[] objectsToUnactivate;
    public GameObject[] UIToShow;
    
    [Header("Sounds")]
    [SerializeField] private AudioSource loseSound;
    [SerializeField] private AudioSource winSound;
    [SerializeField] private AudioSource tickSound;
    
    [Header("Rounds Control")]
    [SerializeField] private int startCount = 2;
    [SerializeField] private int currentRound = 0;
    [SerializeField] private int maxRoundsCount = 5;

    [Header("Bools")]
    [SerializeField] private bool showingPattern = false;
    [SerializeField] private bool waitingForResult = false;

    [Header("Lists")]
    [SerializeField] private List<int> correctOrder = new List<int>();
    [SerializeField] private List<int> currentSelectedOrder = new List<int>();

    void Update()
    {
        if (showingPattern)
        {
            showingPattern = false;
            StartCoroutine(ShowPattern());
        }
        else if (waitingForResult)
        {
            if(currentSelectedOrder.Count == correctOrder.Count)
            {
                if (currentSelectedOrder.SequenceEqual(correctOrder))
                {
                    NextRound();
                }
                else
                {
                    StartCoroutine(Lose());
                }
            }
        }
    }

    public void StartPuzzle()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().interactable = false;
            button.GetComponent<Image>().color = Color.white;
        }

        //Create first 2 numbers
        for (int i = 1; i < startCount; i++)
        {
            correctOrder.Add(Random.Range(0, 8));
        }
        showingPattern = true;
    }

    IEnumerator ShowPattern()
    {
        DeactivateButtons();

        for (int i = 0; i < correctOrder.Count; i++)
        {
            Image btnImage = buttons[correctOrder.ElementAt(i)].GetComponent<Image>();
            yield return new WaitForSeconds(0.2f);
            tickSound.pitch = Random.Range(1.0f, 3.0f);
            tickSound.Play();
            btnImage.color = Color.blue;
            yield return new WaitForSeconds(0.5f);
            btnImage.color = Color.white;
        }

        ActivateButtons();
        waitingForResult = true;
    }

    void NextRound()
    {
        currentRound++;
        ResetSelectedOrder();
        if (currentRound <= maxRoundsCount)
        {
            correctOrder.Add(Random.Range(0, 8));
            showingPattern = true;
        }
        else
            StartCoroutine(Win());
    }

    IEnumerator Win()
    {
        winSound.Play();
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().interactable = false;
            button.GetComponent<Image>().color = Color.green;
        }
        yield return new WaitForSeconds(1f);

        UnactiveObjects();
        
        puzzleToDeactivate.activate = false;
        ClosePuzzle();
    }

    IEnumerator Lose()
    {
        ResetPuzzle();
        loseSound.Play();
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().interactable = false;
            button.GetComponent<Image>().color = Color.red;
        }
        yield return new WaitForSeconds(2f);
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Image>().color = Color.white;
        }
        StartPuzzle();
    }

    void ActivateButtons()
    {
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().interactable = true;
        }
    }

    void DeactivateButtons()
    {
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().interactable = false;
        }
    }

    void ResetPuzzle()
    {
        correctOrder = new List<int>();
        ResetSelectedOrder();

        currentRound = 0;
        waitingForResult = false;
        showingPattern = false;
    }
    
    void ResetSelectedOrder()
    {
        currentSelectedOrder.Clear();
    }

    public void ClosePuzzle() { 
        ResetPuzzle(); 
        this.gameObject.SetActive(false); 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        foreach(GameObject obj in UIToShow)
        {
            obj.SetActive(true);
        }
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().doingPuzzle = false;
    }
    
    public void SetValue(int value) { currentSelectedOrder.Add(value); }
    
    void UnactiveObjects()
    {
        foreach (GameObject obj in objectsToUnactivate)
        {
            Animator objAnim = obj.GetComponent<Animator>();
            if (objAnim != null)
            {
                objAnim.SetBool("Open", true);
            }
            else
            {
                obj.SetActive(false);
            }
        }
    }
}
