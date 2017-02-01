using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Rotate : MonoBehaviour {
        public Button YourButton;

        private void Start()
        {
            //Button btn = this.GetComponent<Button>();
            YourButton.onClick.AddListener(TaskOnClick);
        }

        private void TaskOnClick()
        {
            transform.Rotate(0, 0, -90);
            Debug.Log("You have clicked the button!");
        }
    }
}
