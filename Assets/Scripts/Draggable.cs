using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

        public string hand;
        internal Transform _returnParent = null;
        private GameObject _draggedGameObject = null;
        void Start()
        {

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
     
            
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
