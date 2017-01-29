using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardHandler : MonoBehaviour {
    public int rows, columns;
    public GameObject prefab;
    public GameObject[,] boardArray;
    
    void Start() {
        boardArray = new GameObject[columns, rows];
        Transform canvas = this.transform.parent;
        GameObject board = GameObject.FindGameObjectWithTag("Board");
        RectTransform rect = board.GetComponent<RectTransform>();
        GridLayoutGroup grid = board.GetComponent<GridLayoutGroup>();

        float height = canvas.transform.GetComponent<RectTransform>().rect.height;  //Canvas Size
        float spacing = 5F;
        float boardHeight = height * 0.90F;
        float tileHeight = boardHeight / columns - spacing;   //Tile size = board size / column size

        rect.sizeDelta = new Vector2(boardHeight, boardHeight);
        grid.cellSize = new Vector2(tileHeight, tileHeight);

        Debug.Log(rows + "  " + columns);

        for (int i = 0; i < columns; i++) {
            for (int j = 0; j < rows; j++) {
                GameObject node = Instantiate(prefab);
                node.transform.SetParent(this.transform);
                boardArray[i, j] = node;
            }
        }
    }
}
