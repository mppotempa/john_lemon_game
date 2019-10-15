using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
    private int soulsLeft = 13;
    public Text leftText;
    public GameObject exit;
    public Text timeText;

    float m_TimePassed;

    Vector3 m_Movement;
    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Quaternion m_Rotation = Quaternion.identity;

    //all variables for enemies
    public GameObject gargoyle1;
    public GameObject gargoyle2;
    public GameObject gargoyle3;
    public GameObject ghost1;
    public GameObject ghost2;
    public GameObject ghost3;
    public GameObject ghost4;
    
    //call another script
    GameEnding gameEnding;


    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
        setLeftText();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);

        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

    }

    private void Update()
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
        if (hour == 0)
        {
            hour += 12;
        }

        //AM or PM
        string ampm = "AM";
        if (((m_TimePassed / 60f) % 24f) > 12f)
        {
            ampm = "PM";
        }

        //output time
        if (hour == 7)
        {
            gameEnding.CaughtPlayer();
        }
        else
        {
            timeText.text = hour.ToString() + ":" + minString + ampm;
        }

    }
    private void OnAnimatorMove()
    {
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation(m_Rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if Lemon runs into a soul
        if (other.gameObject.CompareTag("Soul"))
        {
            //hide the soul bit
            other.gameObject.SetActive(false);
            soulsLeft--;
            setLeftText();
            //check to see what to activate
            if(soulsLeft == 0)
            {
                //activate the end of the game
                exit.SetActive(true);
                gargoyle3.SetActive(true);
            }
            else if (soulsLeft == 1)
            {
                gargoyle1.SetActive(true);
            }
            else if (soulsLeft == 3)
            {
                ghost2.SetActive(true);
            }
            else if (soulsLeft == 5)
            {
                gargoyle2.SetActive(true);
            }
            else if (soulsLeft == 7)
            {
                ghost4.SetActive(true);
            }
            else if (soulsLeft == 9)
            {
                ghost3.SetActive(true);
            }
            else if (soulsLeft == 11)
            {
                ghost1.SetActive(true);
            }
        }

    }

    void setLeftText()
    {
        if(soulsLeft > 0)
        {
            leftText.text = "Souls left: " + soulsLeft.ToString();
        }
        else
        {
            leftText.text = "Make it back to the starting point to escape!";
        }
    }
}

