using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class DropArea : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {


        public void OnDrop(PointerEventData eventData)
        {
            var d = eventData.pointerDrag.GetComponent<Draggable>();    //Get the tile dragged
            if (d != null)
            {
                if (gameObject == GameHandler.gameHandler.resHand && (d.GetComponent<TileData>().type.Contains("Resistor") || d.GetComponent<TileData>().type == ResistileServer.GameTileTypes.solder))
                {
                    d._returnParent = transform;
                }
                else if (gameObject == GameHandler.gameHandler.wireHand && d.GetComponent<TileData>().type.Contains("Wire"))
                {
                    d._returnParent = transform;
                }
                else if(gameObject != GameHandler.gameHandler.resHand && gameObject != GameHandler.gameHandler.wireHand)
                {
                    d._returnParent = transform;
                    int[] coord = BoardHandler.CoordinatesOf(gameObject);
                }

            }
            if (gameObject != GameHandler.gameHandler.resHand && gameObject != GameHandler.gameHandler.wireHand)    //If tile dropped on a board node
            {
                if (d.GetComponent<TileData>().type == ResistileServer.GameTileTypes.solder)
                {
                    GameHandler.gameHandler.solderTile = d.gameObject;
                }
                else
                {
                    GameHandler.gameHandler.currentTile = d.gameObject;
                    GameHandler.gameHandler.setAllTileDrag(false);
                }
            }
            else if(gameObject == GameHandler.gameHandler.resHand || gameObject == GameHandler.gameHandler.wireHand)    //If drop area is a res hand
            {
                GameHandler.gameHandler.currentTile = null;
                GameHandler.gameHandler.setAllTileDrag(true);
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