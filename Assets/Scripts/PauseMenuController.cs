using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    bool attemptedEndDay;
    [SerializeField]
    GameObject PauseButton;
    [SerializeField]
    TextMeshProUGUI EndDayText;
    [SerializeField]
    GameObject PauseScreen;
    [SerializeField]
    Scrollbar VolumeSlider;
    [SerializeField]
    GameObject TimerUI;




    private void Start()
    {
        VolumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.5f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseScreen.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void EnablePause(bool enable = true)
    {
        PauseButton.SetActive(enable);
    }

    public void ResumeGame()
    {
        PauseScreen.SetActive(false);
        TimerUI.GetComponent<Timer>().ResumeTimer();
        EnablePause(true);
    }

    public void PauseGame()
    {
        PauseScreen.SetActive(true);
        TimerUI.GetComponent<Timer>().PauseTimer();
        EnablePause(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void EndDay()
    {
        if (attemptedEndDay)
        {
            GameManager.instance.EndDay();
        }
        else
        {
            EndDayText.text = "<color=\"red\">Confirm End Day</color>";
            attemptedEndDay = true;
        }
    }

    public void SetVolume(float volume)
    {
        AudioManager.instance.SetVolume(volume);
        PlayerPrefs.SetFloat("Volume", volume);
    }
}
