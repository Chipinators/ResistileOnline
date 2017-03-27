using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {
    public Sprite[] imageArray;
    public int currentImage;
    private GameObject imagePanel;

    private void Start()
    {
        imagePanel = GameObject.FindGameObjectWithTag("ImagePanel");

    }


    public void onRightDown()
    {
        Debug.Log("RIGHT BUTTON PRESSED");
        currentImage++;
        updateImagePanel();
    }

    public void onLeftDown()
    {
        Debug.Log("LEFT BUTTON PRESSED");
        currentImage--;
        updateImagePanel();
        
    }

    private void updateImagePanel()
    {
        SpriteRenderer sr = imagePanel.GetComponent<SpriteRenderer>();
        sr.sprite = imageArray[currentImage];
    }
}
