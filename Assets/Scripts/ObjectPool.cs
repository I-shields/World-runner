//============================================================
// Author: Isaac Shields
// Date  : 10-12-2024
// Desc  : Object pool so the engine doesn't have to
//         contently create and destroy 100s of game objects          
//============================================================
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject grassBlock;
    public int poolSize = 50;
    private List<GameObject> pooledItems;

    void Start()
    {
    }

    public void makePool()
    {
        //create the pool and spawn in the initial blocks
        pooledItems = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(grassBlock);
            obj.SetActive(false);
            pooledItems.Add(obj);
        }
    }
    public GameObject getObjectFromPool()
    {
        //return an inactive game object to use if one is available
        //creates a new game object if non are available
        foreach (GameObject obj in pooledItems)
        {
            if(!obj.activeInHierarchy)
            {
                obj.layer = LayerMask.NameToLayer("Ground");
                obj.SetActive(true);
                return obj;
            }
        }

        GameObject newItem = Instantiate(grassBlock);
        newItem.SetActive(true);
        pooledItems.Add(newItem);
        newItem.layer = LayerMask.NameToLayer("Ground");
        return newItem;
    }

    public void ReturnItem(GameObject item)
    {
        //returns a game object to the pool when no longer being used
        //clears any attributes from the object
        item.transform.SetParent(null);
        if(item.GetComponent<BoxCollider2D>() != null)
        {
            Destroy(item.GetComponent<BoxCollider2D>());
        }
        if(item.tag == "diamond" || item.tag == "weight")
        {
            Destroy(item);
        }
        // Clear all children
        while (item.transform.childCount > 0)
        {
            GameObject child = item.transform.GetChild(0).gameObject;
            child.transform.SetParent(null);
            child.SetActive(false);
            if(!pooledItems.Contains(child) && child.tag != "diamond")
            {
                pooledItems.Add(child);
            }
        }

        if(!pooledItems.Contains(item))
        {
            pooledItems.Add(item);
        }

        item.SetActive(false);
    }

}
