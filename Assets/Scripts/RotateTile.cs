using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class RotateTile : MonoBehaviour
    {
        internal int rotation = 0;
        public Button YourButton;

        private void Start()
        {
            //Button btn = this.GetComponent<Button>();
            YourButton.onClick.AddListener(TaskOnClick);
        }

        public void TaskOnClick()
        {
            //rotation = (rotation + 1) % 4;
            //transform.Rotate(0, 0, -90);
            //GameNodeAdapter adapter = GetComponent<GameNodeAdapter>();
            //adapter.Rotate();
            //Debug.Log("You have clicked the button!");
            Transform background = this.transform.FindChild("Background");
            background.Rotate(0, 0, -90);

        }
    }
}
