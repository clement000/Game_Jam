using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletCounter : MonoBehaviour
{
    public Text bulletDisplay;
    public int bulletNumber;

    public int BulletNumber
    {
        get { return bulletNumber; }
        set { bulletNumber = value; }
    }

    private void Start()
    {
        bulletNumber = 10;
    }

    private void Update()
    {
        bulletDisplay.text = bulletNumber.ToString();
    }


}
