using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class DropArea : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
    

        public void OnDrop(PointerEventData eventData)
        {
            var d = eventData.pointerDrag.GetComponent<Draggable>();    //Get the node the tile was dropped on
            if (d != null)
            {
                d._returnParent = transform;
                int[] coord = BoardHandler.CoordinatesOf(gameObject);

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