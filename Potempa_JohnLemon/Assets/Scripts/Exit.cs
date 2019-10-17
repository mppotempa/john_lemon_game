using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;

    public GameEnding gameEnding;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            print("Reached Exit");
            gameEnding.IsAtExit();
            print("Done");
        }
    }
}
