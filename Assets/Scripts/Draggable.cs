using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

        internal Transform returnParent = null;
        void Start()
        {

        }

        public void OnBeginDrag(PointerEventData eventData)
        { 
            returnParent = gameObject.transform.parent;
            gameObject.transform.SetParent(GameObject.Find("Canvas").transform);
            gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            gameObject.transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            gameObject.transform.SetParent(returnParent);
            gameObject.transform.SetAsFirstSibling();
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        
    }
}
