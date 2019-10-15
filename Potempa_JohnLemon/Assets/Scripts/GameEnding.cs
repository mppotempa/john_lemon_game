using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;

    public GameObject player;
    public CanvasGroup exitBackgroundImageCanvasGroup;
    public CanvasGroup caughtBackgroundImageCanvasGroup;
    public AudioSource exitAudio;
    public AudioSource caughtAudio;
    public Text timeText;

    float m_Timer;
    float m_TimePassed;
    bool m_HasAudioPlayed;
    bool m_IsPlayerAtExit;
    bool m_IsPlayerCaught;

    private void Update()
    {
        if (m_IsPlayerAtExit)
        {
            EndLevel(exitBackgroundImageCanvasGroup, false, exitAudio);
        }
        else if (m_IsPlayerCaught)
        {
            EndLevel(caughtBackgroundImageCanvasGroup, true, caughtAudio);
        }
        else
        {
            //display time
            m_TimePassed += Time.deltaTime;

            //min
            int min = Mathf.FloorToInt(m_TimePassed % 60f);
            string minString = min.ToString();
            if (min < 10)
            {
                minString = "0" + minString;

            }

            //hour
            int hour = (Mathf.FloorToInt(m_TimePassed / 60f)) % 12;
            if(hour == 0)
            {
                hour += 12;
            }

            //AM or PM
            string ampm = "AM";
            if(((m_TimePassed/60f) %  24f) > 12f)
            {
                ampm = "PM";
            }

            //output time
            if (hour == 7)
            {
                CaughtPlayer();
            }
            else
            {
                timeText.text = hour.ToString() + ":" + minString + ampm;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            m_IsPlayerAtExit = true;
        }
    }

    public void CaughtPlayer()
    {
        m_IsPlayerCaught = true;
    }

    void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource)
    {
        if (!m_HasAudioPlayed)
        {
            audioSource.Play();
            m_HasAudioPlayed = true;
        }
        m_Timer += Time.deltaTime;
        imageCanvasGroup.alpha = m_Timer / fadeDuration;

        if(m_Timer > fadeDuration + displayImageDuration)
        {
            if (doRestart)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                Application.Quit();
            }
        }
    }
}
