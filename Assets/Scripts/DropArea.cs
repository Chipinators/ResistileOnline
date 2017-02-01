using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class DropArea : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {


        public void OnDrop(PointerEventData eventData)
        {
            var d = eventData.pointerDrag.GetComponent<Draggable>();
            if(d!= null)
            {
                var coord = BoardHandler.CoordinatesOf(gameObject);
                Debug.Log("Coordinates: " + "X - " + coord[0] + " Y - " + coord[1]);
                d._returnParent = transform;
            }
       
        
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
        
        }

        public void OnPointerExit(PointerEventData eventData)
        {
        
        }
    }
}
