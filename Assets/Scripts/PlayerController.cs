using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    public float moveSpeed = 3.4f;
    private float inputMovement;
    public float jumpForce = 5f;
    private int diamondCount = 0;
    public AudioClip coinPickup;
    public AudioSource playerSpeaker;
    private bool autoWalk = false;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {


        inputMovement = Input.GetAxisRaw("Horizontal");

        if(Input.GetKeyDown(KeyCode.P))
        {
            autoWalk = !autoWalk;
        }

        if(autoWalk)
        {
            playerRb.velocity = new Vector2(1 * moveSpeed, playerRb.velocity.y);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            playerRb.velocity = Vector2.up * jumpForce;
        }

        if(!autoWalk)
        {
            if(inputMovement > 0)
            {
                playerRb.velocity = new Vector2(inputMovement * moveSpeed, playerRb.velocity.y);
            }
        }

        if(inputMovement > 0)
        {
            playerRb.velocity = new Vector2(inputMovement * moveSpeed, playerRb.velocity.y);
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(Resources.FindObjectsOfTypeAll<GameObject>().Length);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "diamond")
        {
            playerSpeaker.PlayOneShot(coinPickup);
            diamondCount++;
            Destroy(other.gameObject);
            Debug.Log(diamondCount);
        }
    }

}
