using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
        public GameObject tilePrefab;
        GameObject placeholder;
        internal Transform returnParent = null;
        void Start()
        {

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            placeholder = Instantiate(tilePrefab);
            placeholder.transform.SetParent(transform.parent);
            placeholder.transform.SetSiblingIndex(gameObject.transform.GetSiblingIndex());
            placeholder.GetComponent<CanvasGroup>().alpha = 0;


            returnParent = gameObject.transform.parent;
            gameObject.transform.SetParent(GameObject.Find("Canvas").transform);
            gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            

        }

        public void OnDrag(PointerEventData eventData)
        {
            gameObject.transform.position = eventData.position;

            int newIndex = placeholder.transform.parent.childCount;

            for (int i = 0; i < placeholder.transform.parent.childCount; i++)
            {
                
                if (transform.position.y > placeholder.transform.parent.GetChild(i).transform.position.y)
                {
                    newIndex = i;
                    if (placeholder.transform.GetSiblingIndex() < newIndex)
                        newIndex--;
                    break;
                }
            }
            placeholder.transform.SetSiblingIndex(newIndex);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            gameObject.transform.SetParent(returnParent);
            if(returnParent.Equals(GameHandler.gameHandler.resHand) || returnParent.Equals(GameHandler.gameHandler.wireHand))
            {   //If return parent is the resistor or wire hand
                gameObject.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
            }   //If return parent is on the board
            else
            {
                gameObject.transform.SetAsFirstSibling();
            }
            
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            DestroyImmediate(placeholder);
        }

        
    }
}
