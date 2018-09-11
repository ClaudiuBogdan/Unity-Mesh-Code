using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameRate : MonoBehaviour {
    int frameCounter = 0;
    float timeCounter = 0.0f;
    float lastFramerate = 0.0f;
    int fps = 0;
    public float refreshTime = 0.5f;
    [SerializeField]
    Text fps_text;

    private void Start()
    {
        fps_text = GetComponentInChildren<Text>(); //Get the text component from the UI children
        Application.targetFrameRate = 60;
    }
    void Update()
    {
        if (timeCounter < refreshTime) //Counts the frames that are given in a certain deltaTime
        {
            timeCounter += Time.deltaTime;
            frameCounter++;
        }
        else
        {
            
            lastFramerate = (float)frameCounter / timeCounter;

            frameCounter = 0;
            timeCounter = 0.0f;
            fps = (int)lastFramerate;
            fps_text.text = fps.ToString();
        }
    }
}
