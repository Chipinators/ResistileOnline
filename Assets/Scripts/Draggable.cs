using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

        internal Transform _returnParent = null;
        private GameObject _draggedGameObject = null;

        public void OnBeginDrag(PointerEventData eventData)
        {
            GameObject newInstance = Instantiate(gameObject);
            newInstance.transform.SetParent(GameObject.Find("Hand").transform);

            newInstance.GetComponent<Rotate>().rotation = gameObject.GetComponent<Rotate>().rotation;
            
            
            _draggedGameObject = gameObject;
            _returnParent = _draggedGameObject.transform.parent;
            _draggedGameObject.transform.SetParent(GameObject.Find("Canvas").transform);
            _draggedGameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _draggedGameObject.transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _draggedGameObject.transform.SetParent(_returnParent);
            GetComponent<CanvasGroup>().blocksRaycasts = true;

        }

        
    }
}
