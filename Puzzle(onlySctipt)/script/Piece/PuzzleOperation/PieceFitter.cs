using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PieceFitter{
    internal void SetRelativePosition(GameObject connectedObj, GameObject addObj, Vector2 relative){
        RectTransform cRTF = connectedObj.GetComponent<RectTransform>();
        RectTransform aRTF = addObj.GetComponent<RectTransform>();
        Transform parent = aRTF.parent;
        aRTF.SetParent(cRTF);
        aRTF.position = new Vector3(cRTF.position.x + relative.x, cRTF.position.y + relative.y, cRTF.position.z);
        aRTF.localScale = Vector3.one;
        
        Vector3 worldPos = aRTF.position;
        aRTF.SetParent(parent);
        aRTF.position = worldPos;
        aRTF.localScale = Vector3.one;
        
    }
    internal Vector2 GetRelativePosition(EdgePosition targetEdge){
        Vector2 relativePos = Vector2.zero;
        switch (targetEdge){
            case EdgePosition.Left:
                relativePos = new Vector2(-75,0);
                return relativePos;
            case EdgePosition.Top:
                relativePos = new Vector2(0, 75);
                return relativePos;
            case EdgePosition.Right:
                relativePos = new Vector2(75, 0);
                return relativePos;
            case EdgePosition.Bottom:
                relativePos = new Vector2(0, -75);
                return relativePos;
        }
        return relativePos;
    }
    /// <summary>
    /// Pieceの接続の可否を返すメソッド
    /// </summary>
    /// <param name="connectedPiece">つながるターゲットとなるピース</param>
    /// <param name="addPiece">新たにつなげるピース</param>
    /// <param name="targetEdge">ターゲットのどのピースにつなげるか</param>
    /// <returns></returns>
    internal bool IsConnectable(PuzzlePiece connectedPiece, PuzzlePiece addPiece, EdgePosition targetEdge){
        var addPieceEdge = GetConnectEdge(targetEdge); // 加えるピースの接続辺を取得
        
        if(connectedPiece.EdgeFilled[targetEdge] == null){
            switch(connectedPiece.Edges[targetEdge]){
                case EdgeType.Concave:
                    if(addPiece.Edges[addPieceEdge] == EdgeType.Convex){
                        return true;
                    } else {
                        return false;
                    }
                case EdgeType.Convex:
                    if(addPiece.Edges[addPieceEdge] == EdgeType.Concave){
                        return true;
                    } else {
                        return false;
                    }

                default:
                    return false;
            }
        } else {
            return false;
        }
    }
    //ターゲットのどこに繋げるかに対して，新たに接続するメソッドのどの辺がつながるかを返すmethod
    private EdgePosition GetConnectEdge(EdgePosition targetEdge){
        switch (targetEdge){
            case EdgePosition.Top:
                return EdgePosition.Bottom;
            case EdgePosition.Bottom:
                return EdgePosition.Top;
            case EdgePosition.Left:
                return EdgePosition.Right;
            case EdgePosition.Right:
                return EdgePosition.Left;
            default:
                return EdgePosition.Top;
        }
    }

    //Edgeを埋める
    internal void FillEdge(PuzzlePiece target, PuzzlePiece myself,  EdgePosition targetEdge){
        EdgePosition selfEdge = GetConnectEdge(targetEdge);
        target.EdgeFilled[targetEdge] = myself.gameObject;
        myself.EdgeFilled[selfEdge] = target.gameObject;
    }
}
