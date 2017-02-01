using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillHand : MonoBehaviour {

    public GameObject prefab;

    void Start (){

        float height = GameObject.Find("Canvas").transform.GetComponent<RectTransform>().rect.height;

        this.GetComponent<RectTransform>().sizeDelta = new Vector2(height*0.15F, height * 0.10F);

        for (int i = 1; i < 6; i++)
        {
            GameObject tile = Instantiate(prefab);
            tile.transform.SetParent(this.transform);
            tile.GetComponentInChildren<Text>().text = i.ToString();
        }

    }
}
