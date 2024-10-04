using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallexBackground : MonoBehaviour
{
    private float length;
    private float lastPos;
    public GameObject player;
    public float moveSpeed;
    void Start()
    {

        lastPos = player.transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float curPos = transform.position.x;
        if(curPos != lastPos)
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            if(Mathf.Abs(player.transform.position.x - transform.position.x) >= 45)
            {
                transform.position = new Vector3(player.transform.position.x + length, transform.position.y, transform.position.z);
            }
        }
        lastPos = transform.position.x;
    }
}
