using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class FillHand : MonoBehaviour {

        public GameObject Prefab;

        private void Start (){

            var height = GameObject.Find("Canvas").transform.GetComponent<RectTransform>().rect.height;

            GetComponent<RectTransform>().sizeDelta = new Vector2(height*0.15F, height * 0.10F);

            for (var i = 1; i < 6; i++)
            {
                var tile = Instantiate(Prefab);
                tile.transform.SetParent(transform);
                tile.GetComponentInChildren<Text>().text = i.ToString();
            }

        }
    }
}
