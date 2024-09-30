using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public Transform groundCheck;
    public float groundDistance = 0.1f;
    public LayerMask groundMask;
    public TextMeshProUGUI uiDiamondCount;
    public TextMeshProUGUI ui_Distance;
    public GameObject start;
    Vector3 velocity;
    bool isGrounded;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        uiDiamondCount.text = "X " + diamondCount.ToString();
    }

    void Update()
    {
        isGrounded = false;
        isGrounded = Physics2D.CircleCast(groundCheck.position, groundDistance, Vector2.down, 0.1f, groundMask);
        float movement = Input.GetAxisRaw("Horizontal");
        if (movement > 0 && !autoWalk)
        {
            playerRb.velocity = new Vector2(movement * moveSpeed, playerRb.velocity.y);
        }
        else if(autoWalk)
        {
            playerRb.velocity = new Vector2(1 * moveSpeed, playerRb.velocity.y);
        }
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            //playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            playerRb.velocity = Vector2.up * jumpForce;
            playerRb.velocity = new Vector2((playerRb.velocity.x / 2), playerRb.velocity.y);
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            autoWalk = !autoWalk;
        }
        ui_Distance.text = "Distance: " + Mathf.RoundToInt(Mathf.Abs(transform.position.x - start.transform.position.x)).ToString();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "diamond")
        {
            playerSpeaker.PlayOneShot(coinPickup);
            diamondCount++;
            Destroy(other.gameObject);
            uiDiamondCount.text = "x " + diamondCount.ToString();
        }
    }

}
