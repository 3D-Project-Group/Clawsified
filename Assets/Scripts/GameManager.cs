using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Popup Control")]
    [SerializeField] private GameObject popUpObj;
    [SerializeField] private Image popUpCurrentImg;
    [SerializeField] private TMP_Text popUpTitle;
    [SerializeField] private TMP_Text popUpText;
    [SerializeField] private Sprite[] popUpImages;

    [HideInInspector] public Queue<Popup> popUpQueue = new Queue<Popup>();

    private void Update()
    {
        if (!GameInfo.showingPopup && popUpQueue.Count > 0)
        {
            ShowPopUp(popUpQueue.Dequeue());
        }
        else if (GameInfo.showingPopup && Input.GetKeyDown(KeyCode.E))
        {
            HidePopUp();
        }
    }

    public void AddPopupToQueue(Popup popUp)
    {
        popUpQueue.Enqueue(popUp);
    }

    void ShowPopUp(Popup popUp)
    {

        popUpTitle.text = popUp.title;
        popUpText.text = popUp.text;
        popUpCurrentImg.sprite = popUpImages[popUp.imgIndex];
        popUpObj.SetActive(true);

        GameInfo.showingPopup = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    void HidePopUp()
    {
        popUpObj.SetActive(false);

        GameInfo.showingPopup = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;

        Time.timeScale = 1f;
    }
}

public class Popup
{
    public string title;
    public string text;
    public int imgIndex;
    public Popup(string title, string text, int imgIndex)
    {
        this.title = title;
        this.text = text;
        this.imgIndex = imgIndex;
    }
}