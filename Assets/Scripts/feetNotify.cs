//============================================================
// Author: Isaac Shields
// Date  : 10-12-2024
// Desc  : plays sounds from the feet speaker when needed
//============================================================
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class feetNotify : MonoBehaviour
{
    public AudioClip feetInWater;
    public AudioClip walkInWater;
    public AudioSource playerSpeaker;
    public AudioSource feetSpeaker;
    public GameObject head;
    private bool playingSound = false;
    private bool inWater = false;

    private void Update()
    {
        if(inWater && !head.GetComponent<headNotify>().underWater && !playingSound)
        {
            feetSpeaker.PlayOneShot(walkInWater);
            feetSpeaker.loop = true;
            playingSound = true;
        }
        if(head.GetComponent<headNotify>().underWater && playingSound || !inWater)
        {
            feetSpeaker.Stop();
            playingSound = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "water")
        {
            playerSpeaker.PlayOneShot(feetInWater);
        }
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "water")
        {
            inWater = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "water")
        {
            inWater = false;
        }
    }
}
