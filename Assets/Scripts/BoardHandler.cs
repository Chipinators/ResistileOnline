using ResistileConsole;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class BoardHandler : MonoBehaviour {
        public int Rows, Columns;
        internal static int RowsStat, ColumnsStat;
        public GameObject Prefab;
        public GameObject StartNode;
        public GameObject EndNode;
        internal static GameObject[,] BoardArray;
        internal static GameControl myGame = new GameControl();

        private void Start() {
            RowsStat = Rows;
            ColumnsStat = Columns;
            BoardArray = new GameObject[Rows, Columns];
            var canvas = transform.parent;
            var board = GameObject.FindGameObjectWithTag("Board");
            var rect = board.GetComponent<RectTransform>();
            var grid = board.GetComponent<GridLayoutGroup>();

            var nodeS = Instantiate(StartNode);
            nodeS.transform.SetParent(transform, false);
            BoardArray[0, 0] = nodeS;
            
            for (var y = 0; y < Columns; y++)
            for (var x = 0; x < Rows; x++)
            {
                if(y == 0 && x == 0)
                    continue;
                if (y == Columns - 1 && x == Rows - 1)
                    continue;
                var node = Instantiate(Prefab);
                node.name += x + " " + y;
                node.transform.SetParent(transform, false);
                BoardArray[x, y] = node;
            }


            var nodeE = Instantiate(EndNode);
            nodeE.transform.Rotate(0, 0, -180);
            nodeE.transform.SetParent(transform, false);
            BoardArray[Rows - 1, Columns - 1] = nodeE;

        }

        public static int[] CoordinatesOf(GameObject value)
        {
            for (var x = 0; x < ColumnsStat; x++)
            for (var y = 0; y < RowsStat; y++)
                if (BoardArray[x, y].GetInstanceID() == value.GetInstanceID())
                    return new int[] { x, y};
            return new int[] { -1, -1 };
        }
    }
}
