using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Music Control")]
    [SerializeField] private AudioSource bgMusic;
    [SerializeField] private float bgMusicMaxVolume;
    [SerializeField] private AudioSource[] gameAudioSources;
    
    [Header("Popup Control")]
    [SerializeField] private GameObject popUpObj;
    [SerializeField] private Image popUpCurrentImg;
    [SerializeField] private TMP_Text popUpTitle;
    [SerializeField] private TMP_Text popUpText;
    [SerializeField] private Sprite[] popUpImages;

    [HideInInspector] public Queue<Popup> popUpQueue = new Queue<Popup>();

    private void Awake()
    {
        gameAudioSources = FindObjectsOfType<AudioSource>();
    }

    void Update()
    {
        if(bgMusic.volume < bgMusicMaxVolume)
            FadeInMusic();
        
        if (!GameInfo.showingPopup && popUpQueue.Count > 0)
        {
            ShowPopUp(popUpQueue.Dequeue());
        }
        else if (GameInfo.showingPopup && Input.GetKeyDown(KeyCode.E))
        {
            HidePopUp();
        }
    }

    public void PauseGameSounds()
    {
        foreach (AudioSource audio in gameAudioSources)
        {
            if(audio.isPlaying)
                audio.Pause();
        }
    }
    
    public void UnpauseGameSounds()
    {
        foreach (AudioSource audio in gameAudioSources)
        {
            if(!audio.isPlaying)
                audio.UnPause();
        }
    }

    void FadeInMusic()
    {
        bgMusic.volume += 0.01f;
    }
    
    void FadeOutMusic()
    {
        bgMusic.volume -= 0.01f;
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