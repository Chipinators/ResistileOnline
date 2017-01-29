using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetDimensions : MonoBehaviour {
    
    void Start()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        GameObject board = GameObject.FindGameObjectWithTag("Board");
        GridLayoutGroup grid = board.GetComponent<GridLayoutGroup>();
        int columns = grid.constraintCount;
        float height = canvas.transform.GetComponent<RectTransform>().rect.height;
        float tileHeight = (height * 0.90F) / columns - 5F;

        this.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(tileHeight, tileHeight);

    }

}
