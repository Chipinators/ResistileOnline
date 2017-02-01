using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class BoardHandler : MonoBehaviour {
        public int Rows, Columns;
        public static int RowsStat, ColumnsStat;
        public GameObject Prefab;
        public static GameObject[,] BoardArray;


        private void Start() {
            RowsStat = Rows;
            ColumnsStat = Columns;
            BoardArray = new GameObject[Columns, Rows];
            var canvas = transform.parent;
            var board = GameObject.FindGameObjectWithTag("Board");
            var rect = board.GetComponent<RectTransform>();
            var grid = board.GetComponent<GridLayoutGroup>();

            var height = canvas.transform.GetComponent<RectTransform>().rect.height;  //Canvas Size
            var spacing = 3F;
            var boardHeight = height * 0.90F;
            var tileHeight = boardHeight / Columns - spacing;   //Tile size = board size / column size

            rect.sizeDelta = new Vector2(boardHeight, boardHeight);
            grid.cellSize = new Vector2(tileHeight, tileHeight);

            Debug.Log(Rows + "  " + Columns);

            for (var i = 0; i < Columns; i++)
            for (var j = 0; j < Rows; j++) {
                var node = Instantiate(Prefab);
                node.transform.SetParent(transform);
                BoardArray[i, j] = node;
            }
        }

        public static int[] CoordinatesOf(GameObject value)
        {
            for (var x = 0; x < ColumnsStat; ++x)
            for (var y = 0; y < RowsStat; ++y)
                if (BoardArray[x, y].GetInstanceID() == value.GetInstanceID())
                    return new int[] { x, y};
            return new int[] { -1, -1 };
        }
    }
}
