using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShufflePiece : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private PieceManager pieceManager;
    [SerializeField]
    private Transform Inventory;
    [SerializeField]
    private Transform HiddenField;

    public void OnPointerClick(PointerEventData eventData){
        Debug.Log("clicked");
        while(Inventory.childCount > 0){
            Transform piece = Inventory.GetChild(0);
            piece.SetParent(HiddenField);
        }
        int moveCount = Mathf.Min(HiddenField.childCount, 4);
        for(int i = 0; i < moveCount; i++){
            Transform piece = HiddenField.GetChild(0);
            piece.SetParent(Inventory);
        }
    }
}
