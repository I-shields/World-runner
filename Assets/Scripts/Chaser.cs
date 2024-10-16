//============================================================
// Author: Isaac Shields
// Date  : 10-12-2024
// Desc  : logic for the rock dropper
//============================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : MonoBehaviour
{
    private float timer = 0;
    public GameObject rock;
    public int timeBetweenFalls = 6;

    private void Update()
    {
        //spawn a rock at the interval
        //destroy any rocks before spawning a new one
        timer += Time.deltaTime;

        if(timer >= timeBetweenFalls)
        {
            if(transform.childCount > 0)
            {
                for(int i = 0; i < transform.childCount; i++)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }
            }
            GameObject obj = Instantiate(rock, transform.position, Quaternion.identity);
            obj.transform.SetParent(transform);
            timer = 0;
        }
    }
}
