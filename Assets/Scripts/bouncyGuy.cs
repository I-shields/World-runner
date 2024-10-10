using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bouncyGuy : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    private bool isGrounded = true;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if(Mathf.Abs(player.transform.position.x - transform.position.x) < 100)
        {
            isGrounded = false;
            isGrounded = Physics2D.CircleCast(gameObject.transform.position, 0.5f, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
            if(isGrounded)
            {
                rb.velocity = Vector2.up * 9;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
