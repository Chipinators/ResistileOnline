using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rotate : MonoBehaviour {
    public Button yourButton;

    void Start()
    {
        //Button btn = this.GetComponent<Button>();
        yourButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        this.transform.Rotate(0, 0, -90);
        Debug.Log("You have clicked the button!");
    }
}
