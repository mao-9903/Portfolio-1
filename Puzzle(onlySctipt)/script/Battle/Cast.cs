using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cast : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData){
        PiecePathsGenerator piecePathsGenerator= new PiecePathsGenerator();
        piecePathsGenerator.GenerateQueue();

        //各pathについてPieceを順次取り出し，Actionを実行する   
    }
}
