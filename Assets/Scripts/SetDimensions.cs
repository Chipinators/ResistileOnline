using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class SetDimensions : MonoBehaviour {
        private void Start()
        {
            var canvas = GameObject.Find("Canvas").transform;
            var board = GameObject.FindGameObjectWithTag("Board");
            var grid = board.GetComponent<GridLayoutGroup>();
            var columns = grid.constraintCount;
            var height = canvas.transform.GetComponent<RectTransform>().rect.height;
            var tileHeight = height * 0.90F / columns - 5F;

            transform.GetComponent<RectTransform>().sizeDelta = new Vector2(tileHeight, tileHeight);

        }

    }
}
