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
        }
        else
        {
            waitingPanel.SetActive(false);
            foundPanel.SetActive(true);
        }
    }

    public void changeWaiting()
    {
        isWaiting = !isWaiting;
    }
}
