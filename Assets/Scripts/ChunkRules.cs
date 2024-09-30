using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkRules : MonoBehaviour
{
    private Sprite dirtBlock;
    private Sprite stoneBlock;
    private Sprite sandSprite;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        if(transform.childCount > 0)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            dirtBlock = Resources.Load<Sprite>("dirtSprite");
            stoneBlock = Resources.Load<Sprite>("stoneSprite");
            sandSprite = Resources.Load<Sprite>("sandSprite");
            if(transform.position.y <= -0.5f)
            {
                spriteRenderer.sprite = sandSprite;
            }
            gameRules();
        }
    }

    private void gameRules()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform block = transform.GetChild(i);
            spriteRenderer = transform.GetChild(i).GetComponent<SpriteRenderer>();
            float gradient = Mathf.Lerp(0.95f, 0.05f, (float)i / (transform.childCount - 1));

            if(Random.value < gradient)
            {
                spriteRenderer.sprite = dirtBlock;
            }
            else
            {
                spriteRenderer.sprite = stoneBlock;
            }

        }
    }
}
