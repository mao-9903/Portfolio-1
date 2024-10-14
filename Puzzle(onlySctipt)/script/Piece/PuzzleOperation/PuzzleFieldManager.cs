using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Pu
public class PuzzleFieldManager : MonoBehaviour
{
    internal List<GameObject> PiecesInField = new List<GameObject>();
    internal GameObject additionalPiece;
    internal int manaBonus = 0;
    [SerializeField]
    private TextMeshProUGUI manaText;
    [SerializeField]
    private Transform inventoryPanel;

    //newPieceは新たに接続するPiece
    //Costに関して接続の可否を確認
    internal bool CheckTotalCost(GameObject newPiece){
        int totalCost = CalcCurrentCost();
        totalCost -= newPiece.GetComponent<PuzzlePiece>().cost;
        if(totalCost >= 0){
            return true;
        } else {
            
            return false;
        }
    }
    //コストを記述
    internal void WriteCostText(int cost){
        manaText.text = cost.ToString();
    }

    //現在のコストを判定
    internal int CalcCurrentCost(){
        int currentCost = 0;
        currentCost += manaBonus;
        if(PiecesInField.Count > 0){
            foreach(GameObject piece in PiecesInField){
                currentCost -= piece.GetComponent<PuzzlePiece>().cost;
            }
        }
        
        return currentCost;
    }
    
    internal void AddPiece(){
        PiecesInField.Add(additionalPiece);
        additionalPiece = null;
    }

    internal void RemovePiece(){
        int totalCost = CalcCurrentCost();
        if(totalCost <0){
            foreach(GameObject obj in PiecesInField){
                obj.GetComponent<RectTransform>().SetParent(inventoryPanel);
            }
           PiecesInField.Clear(); 
        }

    }
}
