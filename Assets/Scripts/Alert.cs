using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alert : MonoBehaviour
{
    public GameObject alertPanel, alertText;
    private CanvasGroup alertCanvas;
    public float alertTimer;

    // Use this for initialization
    void Start()
    {
        alertTimer = 0.0f;
        alertCanvas = alertPanel.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (alertTimer > 0)
        {
            alertTimer -= Time.deltaTime;
            alertCanvas.alpha = 1;
        }
        else if (alertTimer <= 0)
        {
            alertCanvas.alpha = 0;
        }
    }

    public void alert(string alertStr, float timer, bool isBad)
    {
        alertText.GetComponent<Text>().text = alertStr;
        alertTimer = timer;
        if (isBad)
        {
            alertPanel.GetComponent<Image>().color = new Color32(143, 0, 0, 175);
            alertPanel.GetComponent<Outline>().effectColor = new Color32(255, 0, 0, 130);
        }
        else
        {
            alertPanel.GetComponent<Image>().color = new Color32(0, 35, 23, 175);
            alertPanel.GetComponent<Outline>().effectColor = new Color32(40, 255, 0, 130);
        }
        
    }
}