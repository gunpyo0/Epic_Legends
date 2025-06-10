using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragment : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;



    private void Start()
    {
    }

    public void DropFragment(Item fragment)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();        
        spriteRenderer.sprite = fragment.icon;
    }

}
