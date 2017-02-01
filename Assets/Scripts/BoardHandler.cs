using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardHandler : MonoBehaviour {
    public int rows, columns;
    public static int rowsStat, columnsStat;
    public GameObject prefab;
    public static GameObject[,] boardArray;

    
    void Start() {
        rowsStat = rows;
        columnsStat = columns;
        boardArray = new GameObject[columns, rows];
        Transform canvas = this.transform.parent;
        GameObject board = GameObject.FindGameObjectWithTag("Board");
        RectTransform rect = board.GetComponent<RectTransform>();
        GridLayoutGroup grid = board.GetComponent<GridLayoutGroup>();

        float height = canvas.transform.GetComponent<RectTransform>().rect.height;  //Canvas Size
        float spacing = 3F;
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

    public static int[] CoordinatesOf(GameObject value)
    {
        for (int x = 0; x < columnsStat; ++x)
        {
            for (int y = 0; y < rowsStat; ++y)
            {
                if (boardArray[x, y].GetInstanceID() == value.GetInstanceID())
                    return new int[] { x, y};
            }
        }
        return new int[] { -1, -1 };
    }
}
