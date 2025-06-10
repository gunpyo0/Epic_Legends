using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
    [SerializeField] private GameObject fragmentPrefab;
    [SerializeField] private Item lastFragColor;
    public Item LastFragColor { set { lastFragColor = value; } }

    public void GiveReward()
    {
        GameObject temp = Instantiate(fragmentPrefab, transform.position, Quaternion.identity);
        temp.GetComponent<Fragment>().DropFragment(lastFragColor);
    }

}
