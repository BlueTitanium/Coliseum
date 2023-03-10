using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager gm;

    public CinemachineVirtualCamera startCam;
    public Animation startAnimation;
    public bool isPaused = true;
    bool menuOn = false;
    public bool isStarted = false;
    public Animation menuAnimation;
    public Animation loseAnimation;
    public bool lost = false;
    public TextMeshProUGUI[] rounds;
    public TextMeshProUGUI[] kills;
    public int killCount = 0;

    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public AudioMixerGroup masterMixerGroup;
    public AudioMixerGroup musicMixerGroup;
    public AudioMixerGroup sfxMixerGroup;

    public AudioSource source;
    public AudioSource sfxAud;
    public AudioClip[] clips;
    // Start is called before the first frame update
    void Start()
    {
        source.loop = true;
        killCount = 0;
        startCam.Priority = 11;
        gm = this;
        isPaused = true;
        isStarted = false;
        lost = false;
        masterVolume = Settings.MasterVolume;
        musicVolume = Settings.MusicVolume;
        sfxVolume = Settings.SFXVolume;
        masterSlider.value = masterVolume;
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
        source.clip = clips[0];
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuOn)
            {
                UnpauseGame();
                
            } else
            {
                PauseGame();
                
            }
        }
    }

    public void StartGame()
    {
        startAnimation.Play();
        startCam.Priority = 9;
        isPaused = false;
        isStarted = true;
        source.clip = clips[1]; // game music
        source.Play();
    }

    public void PauseGame()
    {
        sfxAud.PlayOneShot(clips[2]);
        PlayerController.p.PausePlayer();
        menuAnimation["Menu_Open"].speed = 1;
        menuAnimation["Menu_Open"].time = 0;
        menuAnimation.Play("Menu_Open");

        menuOn = true;
        if (isStarted)
        {
            isPaused = true;
        }
    }

    public void UnpauseGame()
    {
        sfxAud.PlayOneShot(clips[2]);
        PlayerController.p.UnpausePlayer();
        menuAnimation["Menu_Open"].speed = -1;
        menuAnimation["Menu_Open"].time = menuAnimation["Menu_Open"].length;
        menuAnimation.Play("Menu_Open");

        menuOn = false;
        if (isStarted || !lost)
        {
            isPaused = false;
        }
    }

    public void SetStatistics()
    {
        if (ArenaManager.Instance != null)
        {
            foreach (var a in rounds)
            {
                a.text = ""+(ArenaManager.Instance.round+1);
            }
        }
        foreach (var a in kills)
        {
            a.text = "" + killCount;
        }
    }


    public void LoseScreen()
    {
        if (lost == false)
        {
            DOTween.KillAll();
            source.Stop();
            source.clip = clips[3]; // game music
            source.Play();
            source.loop = false;
            SetStatistics();
            lost = true;
            loseAnimation.Play();
        }
        
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateMixerVolume()
    {
        print("Changed");
        masterMixerGroup.audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 20);
        musicMixerGroup.audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        sfxMixerGroup.audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
    }
    public void OnMasterSliderValueChange(float value)
    {
        masterVolume = value;
        Settings.MasterVolume = masterVolume;
        UpdateMixerVolume();
    }
    public void OnMusicSliderValueChange(float value)
    {
        musicVolume = value;
        Settings.MusicVolume = musicVolume;
        UpdateMixerVolume();
    }

    public void OnSoundEffectsSliderValueChange(float value)
    {
        sfxVolume = value;
        Settings.SFXVolume = sfxVolume;
        UpdateMixerVolume();
    }
}
