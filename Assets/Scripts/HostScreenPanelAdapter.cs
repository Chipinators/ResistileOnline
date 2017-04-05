using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostScreenPanelAdapter : MonoBehaviour {

    public bool isWaiting = true;
    public GameObject waitingPanel, foundPanel;
    private CanvasGroup waitCanvas, foundCanvas;

    void Start()
    {
        waitCanvas = waitingPanel.GetComponent<CanvasGroup>();
        foundCanvas = foundPanel.GetComponent<CanvasGroup>();
    }


    // Update is called once per frame
    void Update()
    {
        if (isWaiting)
        {
            waitingPanel.SetActive(true);
            foundPanel.SetActive(false);
            //waitCanvas.alpha = 1;
            //waitCanvas.blocksRaycasts = true;
            //foundCanvas.alpha = 0;
            //foundCanvas.blocksRaycasts = false;
        }
        else
        {
            waitingPanel.SetActive(false);
            foundPanel.SetActive(true);
            //waitCanvas.alpha = 0;
            //waitCanvas.blocksRaycasts = false;
            //foundCanvas.alpha = 1;
            //foundCanvas.blocksRaycasts = true;
        }
    }

    public void changeWaiting()
    {
        isWaiting = !isWaiting;
    }
}
