using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragment : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    
    [SerializeField] private Item currentFragment;



    private void Start()
    {
    }

    public void DropFragment(Item fragment)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();        
        spriteRenderer.sprite = fragment.icon;
        currentFragment = fragment;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Inventory>().AddItem(currentFragment);
            Destroy(gameObject);
        }
    }

}
