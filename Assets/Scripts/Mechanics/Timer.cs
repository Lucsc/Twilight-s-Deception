using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public GameObject endScreen;

    [SerializeField]
    private float timeDuration = 5f * 60;
    [SerializeField]
    private float timer;

    [SerializeField]
    private TMP_Text text;

    private float flashTimer;
    private float flashDuration = 1f;

    private bool isPaused;
    void Start()
    {
        ResetTimer();
        isPaused = false;
    }

    void Update()
    {
        if (!isPaused)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                UpdateTimerDisplay(timer);
            }
            else
            {
                Flash();
            }
        }
    }

    private void ResetTimer()
    {
        timer = timeDuration;
    }

    public void PauseTimer()
    {
        isPaused = true;
    }

    public void ResumeTimer()
    {
        isPaused = false;
    }

    public void RemoveTime(int seconds)
    {
        timer -= seconds;
        if(timer < 0)
        {
            timer = 0f;
            UpdateTimerDisplay(timer);
            Flash();
        }
        else
            UpdateTimerDisplay(timer);
    }

    private void UpdateTimerDisplay(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        string currentTime;

        if (seconds > 9)
            currentTime = minutes + ":" + seconds;
        else
            currentTime = minutes + ":0" + seconds;
        text.text = currentTime;
    }

    private void Flash()
    {
       if(timer != 0)
        {
            timer = 0;
            UpdateTimerDisplay(timer);
        }

        if (flashTimer <= 0)
        {
            flashTimer = flashDuration;
        }
        else if(flashTimer >= flashDuration / 2)
        {
            flashTimer -= Time.deltaTime;
            SetTextDisplay(false);
        }
        else
        {
            flashTimer -= Time.deltaTime;
            SetTextDisplay(true);
        }
        endScreen.SetActive(true);
    }

    private void SetTextDisplay(bool enabled)
    {
        text.enabled = enabled;
    }
}
