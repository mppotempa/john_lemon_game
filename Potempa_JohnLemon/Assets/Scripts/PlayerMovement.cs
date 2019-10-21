using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
    private int totalSoul = 13;
    private int soulsLeft = 13;
    public Text escapeWarning;
    public GameObject exit;
    public Slider soulBar;

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
    

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
        soulBar.value = 0;
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
            Debug.Log(soulsLeft);
            soulBar.value = totalSoul - soulsLeft;
            Debug.Log(soulBar.value);

            ;
            //check to see what to activate

            if(soulsLeft == 11)
            {
                //activate the end of the game
                exit.SetActive(true);
                gargoyle3.SetActive(true);
                escapeWarning.text = "Make it back to the starting point to escape!";
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
}

