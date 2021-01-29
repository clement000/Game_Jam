using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletCounter : MonoBehaviour
{
    public Text bulletDisplay;
    public int bulletNumber = 10;

    public int BulletNumber
    {
        get { return bulletNumber; }
        set { bulletNumber = value; }
    }

    private void Start()
    {
    }

    private void Update()
    {
        bulletDisplay.text = bulletNumber.ToString();
    }


}
