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
        pooledItems = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(grassBlock);
            obj.SetActive(false);
            obj.tag = "inList";
            pooledItems.Add(obj);
        }
    }
    public GameObject getObjectFromPool()
    {
        Debug.Log(pooledItems.Count);
        foreach (GameObject obj in pooledItems)
        {
            if(!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        GameObject newItem = Instantiate(grassBlock);
        Debug.Log("New Item created");
        newItem.SetActive(true);
        pooledItems.Add(newItem);
        return newItem;
    }

    public void ReturnItem(GameObject item)
    {
        item.transform.SetParent(null);
        if(item.GetComponent<BoxCollider2D>() != null)
        {
            Destroy(item.GetComponent<BoxCollider2D>());
        }
        if(item.tag == "diamond")
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
