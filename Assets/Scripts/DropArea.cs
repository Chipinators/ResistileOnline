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

                //GameObject coord = BoardHandler.CoordinatesOf(gameObject);
                ////Debug.Log("Coordinates: " + "X - " + coord[0] + " Y - " + coord[1]); 
                //d._returnParent = transform;
                ////Debug.Log(d.tag); 
                //var node = d.GetComponent<GameNodeAdapter>();
                //success = BoardHandler.myGame.AddGameNodeToBoard(node.gameNode, new Coordinates(coord[0], coord[1]));
                //if (success)
                //{
                //    Debug.Log("Successfully added to x " + coord[0] + " y " + coord[1]);
                //}
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