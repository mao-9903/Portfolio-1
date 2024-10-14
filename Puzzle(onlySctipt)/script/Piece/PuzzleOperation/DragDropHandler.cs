using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Linq;


public class DragDropHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private RectTransform rectTransform;
    private Transform inventoryPanel;
    private Transform puzzleField;
    private Transform canvas;
    private PuzzleFieldManager puzzleFieldManager;
    private PuzzlePiece puzzlePiece;
    private Transform HiddenField;
    void Start(){
        inventoryPanel = GameObject.Find("PieceContainer").transform;;
        puzzleField = GameObject.Find("PuzzleField").transform;
        canvas = GameObject.Find("Canvas").transform;
        puzzlePiece = GetComponent<PuzzlePiece>();
        rectTransform = GetComponent<RectTransform>();
        puzzleFieldManager = puzzleField.GetComponent<PuzzleFieldManager>();
        HiddenField = GameObject.Find("HiddenField").transform;
    }

    public void OnPointerClick(PointerEventData eventData){
        if(eventData.button == PointerEventData.InputButton.Right){
            if (Input.GetMouseButton(0)){
                TurnPiece();
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData){
        rectTransform.SetParent(canvas);    
        if(puzzleFieldManager.PiecesInField.Contains(this.gameObject)){
            puzzleFieldManager.PiecesInField.Remove(this.gameObject);   //PuzzleFieldの存在情報を削除
        }
        RemovePieceFromEdge(); //自身と接続Pieceから接続情報を削除
    }

    public void OnDrag(PointerEventData eventData){
        //ひたすら追従
        Vector3 delta = new Vector3(eventData.delta.x, eventData.delta.y, 0);
        rectTransform.position += delta; 
        
    }

    // 後でリファクタ
    public void OnEndDrag(PointerEventData eventData){
        
        PieceFitter pieceFitter = new PieceFitter();//接続に関して座標や接続情報を更新するクラス
        //Piece間の接続の可否
         if(RectTransformUtility.RectangleContainsScreenPoint(puzzleField as RectTransform, Input.mousePosition)){
            if(puzzleFieldManager != null){
                //コストがOkか判定
                if(puzzleFieldManager.CheckTotalCost(this.gameObject)){
                    puzzleFieldManager.additionalPiece = this.gameObject;
                    //PuzzleFieldにすでにPieceが置かれているか判定
                    if(puzzleFieldManager.PiecesInField.Count > 0){
                        // ManaSupplyは一つまで
                        if(puzzlePiece.Type != PuzzlePieceType.ManaSupply){
                            GameObject targetPieceObj = FindClosestPiece(); //最も近いPieceを取得
                            EdgePosition targetEdge = GetTargetEdge(targetPieceObj); //対象となるEdgeを取得
                        
                            PuzzlePiece targetPiece = targetPieceObj.GetComponent<PuzzlePiece>();//対象のPieceを取得
                            //辺同士の接続の可否の判定
                            if(pieceFitter.IsConnectable(targetPiece, puzzlePiece, targetEdge)){
                                SetAnchorPresets.SetAnchorToCenter(gameObject);
                                
                                Vector2 RelativePos = pieceFitter.GetRelativePosition(targetEdge);  //Pieceの相対的な位置関係の取得
                                pieceFitter.SetRelativePosition(targetPieceObj, gameObject, RelativePos);   //相対位置に配置

                                puzzleFieldManager.AddPiece();
                                pieceFitter.FillEdge(targetPiece, puzzlePiece,targetEdge);  //EdgeのPieceの情報の更新
                            } else {
                                rectTransform.SetParent(inventoryPanel);
                            }
                        } else {
                            rectTransform.SetParent(inventoryPanel);
                        }
                    } else {
                        rectTransform.SetParent(puzzleField); //PuzzleFieldにPieceがなければそのまま配置
                        SetAnchorPresets.SetAnchorToCenter(gameObject);
                        puzzleFieldManager.AddPiece();  //Fieldに存在情報を追加
                    }

                } else {
                    rectTransform.SetParent(inventoryPanel);
                }
            }
            
        } else {
            rectTransform.SetParent(inventoryPanel);
        
        }
        puzzleFieldManager.RemovePiece();   //PieceのtotalCostが負になったらすべてクリア
        puzzleFieldManager.WriteCostText(puzzleFieldManager.CalcCurrentCost()); //Costの情報を記述
    }

    private void TurnPiece(){
        //Edgesを回転させる
        Dictionary<EdgePosition, EdgeType> newEdges = new Dictionary<EdgePosition, EdgeType>(){
            {EdgePosition.Top, puzzlePiece.Edges[EdgePosition.Left]},
            {EdgePosition.Right, puzzlePiece.Edges[EdgePosition.Top]},
            {EdgePosition.Bottom, puzzlePiece.Edges[EdgePosition.Right]},
            {EdgePosition.Left, puzzlePiece.Edges[EdgePosition.Bottom]}
        };
        
        puzzlePiece.Edges = newEdges;

        //pieceの画像を回転させる
        Vector3 currentRotation = rectTransform.eulerAngles;
        rectTransform.eulerAngles = new Vector3(currentRotation.x, currentRotation.y, currentRotation.z - 90f);
    }


    private EdgePosition GetTargetEdge(GameObject targetObj){
        EdgePosition targetEdge = EdgePosition.Top;
        RectTransform targetRect = targetObj.GetComponent<RectTransform>();
        Vector2 difference = rectTransform.position - targetRect.position;
        if(difference.y >= difference.x && difference.y >= -difference.x){
            targetEdge = EdgePosition.Top;
        } else if (difference.y <= difference.x && difference.y >= -difference.x){
            targetEdge = EdgePosition.Right;
        } else if (difference.y <= difference.x && difference.y <= -difference.x){
            targetEdge = EdgePosition.Bottom;
        } else if(difference.y >= difference.x && difference.y <= -difference.x){
            targetEdge = EdgePosition.Left;
        }
        
        return targetEdge;
    }

    //PuzzleField内の最も近いPieceを取得
    private GameObject FindClosestPiece(){
        GameObject closest = null;
        float closestDistance = float.MaxValue;
        if(puzzleFieldManager.PiecesInField.Count  != 0){
            foreach(GameObject piece in puzzleFieldManager.PiecesInField){
                float distance = Vector2.Distance(rectTransform.anchoredPosition, piece.GetComponent<RectTransform>().anchoredPosition);
                if(distance < closestDistance){
                    closest = piece;
                    closestDistance = distance;
                }
            }
        }
        
        return closest;
    }

    private void RemovePieceFromEdge(){
        if(puzzleFieldManager.PiecesInField.Count != 0){
            //puzzleFieldにあるすべてのPieceを参照
            foreach(GameObject obj in puzzleFieldManager.PiecesInField){
                PuzzlePiece piece = obj.GetComponent<PuzzlePiece>();
                foreach(var key in piece.EdgeFilled.Keys.ToList()){
                    if(piece.EdgeFilled[key] == this.gameObject){
                        piece.EdgeFilled[key] = null;   //このPieceが接続されていたら接続情報を削除
                    }
                }
            }
        }

        //このPieceの全接続情報を削除
        foreach(var key in puzzlePiece.EdgeFilled.Keys.ToList()){
            puzzlePiece.EdgeFilled[key] = null;
        }
    }
}
