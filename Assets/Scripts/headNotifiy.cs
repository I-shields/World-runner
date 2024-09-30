using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headNotify : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource playerSpeaker;
    public bool underWater = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "water")
        {
            underWater = true;
            playerSpeaker.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "water")
        {
            underWater = false;
        }
    }
}
