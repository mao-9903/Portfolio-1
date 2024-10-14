using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : PuzzlePiece
{
    protected override void SetMember(){
        this.cost = 1;
        this.Type = PuzzlePieceType.Attack;
        this.Edges = new Dictionary<EdgePosition, EdgeType>(){
            {EdgePosition.Top, EdgeType.None},
            {EdgePosition.Right, EdgeType.None},
            {EdgePosition.Bottom, EdgeType.Concave},
            {EdgePosition.Left, EdgeType.None}
        };
    }
    internal override void ActivateEffect(){

    }
}
