using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMagicCircle : PuzzlePiece
{
    protected override void SetMember(){
        this.cost = -3;
        this.Type = PuzzlePieceType.ManaSupply;
        this.Edges = new Dictionary<EdgePosition, EdgeType>(){
            {EdgePosition.Top, EdgeType.Convex},
            {EdgePosition.Right, EdgeType.None},
            {EdgePosition.Bottom, EdgeType.None},
            {EdgePosition.Left, EdgeType.None}
        };

    }

    internal override void ActivateEffect(){

    }
    
}
