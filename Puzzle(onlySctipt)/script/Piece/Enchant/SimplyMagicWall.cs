using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplyMagicWall : PuzzlePiece
{
    protected override void SetMember(){
        this.cost =1;
        this.Type = PuzzlePieceType.Support;
        this.Edges = new Dictionary<EdgePosition, EdgeType>(){
            {EdgePosition.Top, EdgeType.Convex},
            {EdgePosition.Right, EdgeType.None},
            {EdgePosition.Bottom, EdgeType.Concave},
            {EdgePosition.Left, EdgeType.None}
        };
    }
}
