using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class groundMaker : MonoBehaviour
{
    public Transform spawnPos;

    Vector2 spawnLevel;

    private List<GameObject> chunks = new List<GameObject>();
    private GameObject lava;
    private Sprite grassSprite;
    private Sprite dirtSprite;
    private Sprite stoneSprite;
    private Sprite sandSprite;
    private Sprite lavaSprite;

    public Transform player;

    public float seed;
    public ObjectPool itemPool;
    public Tilemap backgroundTiles;

    void Start()
    {
        itemPool.makePool();
        dirtSprite = Resources.Load<Sprite>("dirtSprite");
        stoneSprite = Resources.Load<Sprite>("stoneSprite");
        sandSprite = Resources.Load<Sprite>("sandSprite");
        grassSprite = Resources.Load<Sprite>("grassSprite");
        lavaSprite = Resources.Load<Sprite>("lavaSprite");
        lava = Resources.Load<GameObject>("lavaBlock");
        initialBuild();

    }
    void Update()
    {
        buildWorld();
        if(Mathf.Abs(player.transform.position.x - chunks[0].transform.position.x) > 12)
        {
            itemPool.ReturnItem(chunks[0].gameObject);
            chunks.RemoveAt(0);
        }
        
    }

    private void initialBuild()
    {
        for(int i = -5; i < 10; i++)
        {
            spawnLevel = new Vector2(i, -0.5f);
            GameObject chunk = itemPool.getObjectFromPool();
            chunk.AddComponent<BoxCollider2D>();
            chunk.transform.position = spawnLevel;
            chunks.Add(chunk);
        }
    }

    private void buildWorld()
    {

        //check if we need to add landscape
        if(Mathf.Abs(Vector2.Distance(player.transform.position, chunks[chunks.Count-1].transform.position)) < 50)
        {
            //Get a random number for a height
            int height = UnityEngine.Random.Range(3, 10);

            int changer = 0;

            //Determine if terrain goes up or down
            if(UnityEngine.Random.value < 0.5f)
            {
                changer = -1;
            }
            else
            {
                changer = 1;
            }

            //if the terrain starts going too much in a certain direction "encourage" it to reverse
            if(Mathf.Abs(chunks[chunks.Count-1].transform.position.y) > 10)
            {
                if(UnityEngine.Random.value < (Mathf.Abs(chunks[chunks.Count-1].transform.position.y) * 0.017))
                {
                    if(chunks[chunks.Count-1].transform.position.y < 0)
                    {
                        changer = 1;
                    }
                    else
                    {
                        changer = -1;
                    }
                }
            }


            //Start generating height
            for(int i = 1; i < Mathf.Abs(height); i++)
            {
                //get required positions
                Vector2 lastPos = chunks[chunks.Count - 1].transform.position;
                Vector2 flatArea = new Vector2(lastPos.x + 1, lastPos.y);

                //check to build flat landscape
                if(UnityEngine.Random.value > 0.68f)
                {
                    //How long the "flatness" should strech
                    int distance = UnityEngine.Random.Range(2, 8);

                    //Build the flat area
                    for(int j = 0; j < distance; j++)
                    {
                        //Try spawning lava
                        if(UnityEngine.Random.value > 0.64f && j + 2 < distance && j > 1)
                        {
                            GameObject lavaCube = itemPool.getObjectFromPool();
                            lavaCube.transform.position = flatArea;
                            lavaCube.GetComponent<SpriteRenderer>().sprite = lavaSprite;
                            lavaCube.layer = LayerMask.NameToLayer("playerInteraction");
                            if(lavaCube.GetComponent<BoxCollider2D>() != null)
                            {
                                lavaCube.GetComponent<BoxCollider2D>().isTrigger = true;
                            }
                            else
                            {
                                lavaCube.AddComponent<BoxCollider2D>().isTrigger = true;
                            }
                            lavaCube.tag = "lava";
                            //spawn block below lava
                            Vector2 fillerLoc = new Vector2(flatArea.x, flatArea.y - 1);
                            fillArea(fillerLoc, lavaCube.transform);
                            terrainRules(lavaCube);
                            chunks.Add(lavaCube);
                            flatArea.x += 1;

                        }

                        //spawn in flat area
                        GameObject flatBlock = itemPool.getObjectFromPool();
                        flatBlock.tag = "grass";
                        flatBlock.GetComponent<SpriteRenderer>().sprite = grassSprite;
                        flatBlock.transform.position = flatArea;
                        flatBlock.AddComponent<BoxCollider2D>();
                        Vector2 flatPos = new Vector2(flatArea.x , flatArea.y - 1);
                        //Filler
                        fillArea(flatPos, flatBlock.transform);
                        terrainRules(flatBlock);
                        chunks.Add(flatBlock);

                        flatArea.x += 1;
                        lastPos.x = flatArea.x - 1;
                        lastPos.y = flatArea.y;
                    }
                }
                
                //Adjust the spawn positions for the next block
                lastPos.x += 1;
                lastPos.y += changer;

                //spawn in the next block
                GameObject MainBlock = itemPool.getObjectFromPool();
                MainBlock.tag = "grass";
                MainBlock.GetComponent<SpriteRenderer>().sprite = grassSprite;
                MainBlock.transform.position = lastPos;
                MainBlock.AddComponent<BoxCollider2D>();
                Vector2 blockFiller = new Vector2(lastPos.x, lastPos.y - 1);
                fillArea(blockFiller, MainBlock.transform);
                terrainRules(MainBlock);
                chunks.Add(MainBlock);
            }

        }


        
    }

    private void fillArea(Vector2 pos, Transform parent)
    {
        for(int k = 0; k < 15; k++)
        {
            GameObject UnderLavaFiller = itemPool.getObjectFromPool();
            if(k == 0 && parent.tag == "lava")
            {
                UnderLavaFiller.AddComponent<BoxCollider2D>();
            }
            UnderLavaFiller.GetComponent<SpriteRenderer>().sprite = stoneSprite;
            UnderLavaFiller.transform.position = pos;
            UnderLavaFiller.transform.parent = parent.transform;
            pos.y -= 1;
        }
    }


    private void terrainRules(GameObject item)
    {
        if(item.transform.position.y <= -0.5f && item.tag != "lava")
        {
            item.GetComponent<SpriteRenderer>().sprite = sandSprite;
            item.tag = "sand";
        }

        if(item.transform.childCount > 0)
        {
            for(int i = 0; i < item.transform.childCount; i++)
            {
                Transform block = item.transform.GetChild(i);
                float gradient = Mathf.Lerp(0.95f, 0.05f, (float)i / (item.transform.childCount - 1));

                if(UnityEngine.Random.value < gradient)
                {
                    item.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = dirtSprite;
                }
                else
                {
                    item.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = stoneSprite;
                }

            }
        }

        if(UnityEngine.Random.value < 0.3f && item.tag != "lava")
        {
            GameObject coin = Resources.Load<GameObject>("diamondItem");
            coin = Instantiate(coin, new Vector2(item.transform.position.x, item.transform.position.y + 1.5f), Quaternion.identity);
            coin.transform.parent = item.transform;
        }

    }

    private void spawnRules()
    {


    }
}

