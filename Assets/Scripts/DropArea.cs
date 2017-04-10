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
                {  //If drop area is resHand and tile is resistor or solder, allow placement in resistor hand
                    d.returnParent = transform;
                    GameHandler.gameHandler.currentTile = null;
                    GameHandler.gameHandler.setAllTileDrag(true);
                    if (d.GetComponent<TileData>().type == ResistileServer.GameTileTypes.solder)
                    {
                        GameHandler.gameHandler.solderTile = null;
                    }
                    
                }
                else if (gameObject == GameHandler.gameHandler.wireHand && d.GetComponent<TileData>().type.Contains("Wire"))
                {   //If drop area is wireHand and tile is a wire, allow placement in wire hand
                    d.returnParent = transform;
                    GameHandler.gameHandler.currentTile = null;
                    GameHandler.gameHandler.setAllTileDrag(true);
                }
                else if (gameObject != GameHandler.gameHandler.resHand && gameObject != GameHandler.gameHandler.wireHand)   //If drop area is not the resistance or wire hand
                {
                    if(d.GetComponent<TileData>().type != ResistileServer.GameTileTypes.solder) //If the tile is not a solder
                    {
                        if((GameHandler.gameHandler.solderTile != null && gameObject.transform.childCount == 2) || GameHandler.gameHandler.solderTile == null) //If a solder has been placed and there is a tile on the node
                        {
                            d.returnParent = transform;
                            GameHandler.gameHandler.currentTile = d.gameObject;
                            GameHandler.gameHandler.setAllTileDrag(false);
                        }
                    }
                    else if(gameObject.transform.childCount == 1) //If the node has a tile on it, allow solder to be placed there
                    {
                        d.returnParent = transform;
                        GameHandler.gameHandler.solderTile = d.gameObject;
                        GameHandler.gameHandler.setSolderTileDrag(false);
                    }
                }
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