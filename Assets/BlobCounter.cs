using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlobCounter : MonoBehaviour
{
    public Text GreenCounter;
    public Text RedCounter;
    public int nbGreenBlob;
    public int nbRedBlob;

    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = (float)nbGreenBlob / ((float)nbGreenBlob + (float)nbRedBlob);
        Debug.Log(slider.value);
        GreenCounter.text = nbGreenBlob.ToString();
        RedCounter.text = nbRedBlob.ToString();
    }
}
