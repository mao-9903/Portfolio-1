using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PuzzlePiece : MonoBehaviour
{
    public int UsageCount { get; set; }
    public int cost;
    public PuzzlePieceType Type;
    public Dictionary<EdgePosition, EdgeType> Edges;
    public Dictionary<EdgePosition, GameObject> EdgeFilled;
    internal static EnemyStatus enemyStatus;

    void Start(){
        // SetMember();
        Initialize(3);
        InitializeEdgeFilled();
    }
    
    public void Initialize(int usageCount)
    {
        UsageCount = usageCount;
        SetMember();
        
    }

    protected virtual void SetMember(){}
    

    // 効果を発動するメソッド（派生クラスで実装）
    internal virtual void ActivateEffect(){}

    // ピースの使用を減らすメソッド
    public void UsePiece()
    {
        UsageCount--;
        if (UsageCount <= 0)
        {
            // 使用回数がゼロ以下になった場合、ピースを廃棄する処理
            Dispose();
        }
    }

    // ピースを廃棄するメソッド（派生クラスで実装）
    protected void Dispose(){}
    internal void InitializeEdgeFilled(){
        EdgeFilled = new Dictionary<EdgePosition, GameObject>(){
            {EdgePosition.Left, null},
            {EdgePosition.Right, null},
            {EdgePosition.Top, null},
            {EdgePosition.Bottom, null}
        };
    }
}

