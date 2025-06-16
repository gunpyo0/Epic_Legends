using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{

    [Header("click bool to test")]
    public bool testDash = false;

    [Header("internal use")]
    public playerDashManager dashManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            var mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dashManager.dash(mousePoint,false);
        }
    }
}
