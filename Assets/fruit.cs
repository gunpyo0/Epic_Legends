using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fruit : MonoBehaviour, dashable
{
    SpriteRenderer spriteR;
    public Sprite activeTex;

    private void Awake()
    {
        spriteR = GetComponent<SpriteRenderer>();
    }

    public void triggered()
    {
        spriteR.sprite = activeTex;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
