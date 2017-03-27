using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class FillHand : MonoBehaviour {

        public GameObject Prefab;

        private void Start (){

            for (var i = 1; i < 6; i++)
            {
                var tile = Instantiate(Prefab);
                tile.transform.SetParent(transform, false);
                tile.GetComponentInChildren<Text>().text = i.ToString();
            }

        }
    }
}
