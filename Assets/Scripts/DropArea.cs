﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropArea : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {


    public void OnDrop(PointerEventData eventData)
    {
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if(d!= null)
        {
            int[] coord = BoardHandler.CoordinatesOf(this.gameObject);
            Debug.Log("Coordinates: " + "X - " + coord[0] + " Y - " + coord[1]);
            d.returnParent = this.transform;
        }
       
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
