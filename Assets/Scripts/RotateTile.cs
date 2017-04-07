using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class RotateTile : MonoBehaviour
    {
        public Button YourButton;

        private void Start()
        {
            //Button btn = this.GetComponent<Button>();
            YourButton.onClick.AddListener(TaskOnClick);
        }

        public void TaskOnClick()
        {
            Debug.Log("Rotate called");
            Transform background = this.transform.FindChild("Background");
            background.Rotate(0, 0, -90);

            gameObject.GetComponent<TileData>().rotation = (gameObject.GetComponent<TileData>().rotation + 1) % 4;
        }
    }
}
