using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PuzzlePieceData{
    public int id;
    public int UsageCount;
    public List<EdgeData> Edges;
    public string pieceClass;

}

[System.Serializable]
public class EdgeData
{
    public EdgePosition Position;
    public EdgeType Type;
}

[System.Serializable]
public class SerializationWrapper<T>{
    public List<T> Items;
    public SerializationWrapper(List<T> items) { Items = items; }
}

public enum PuzzlePieceType
{
    ManaSupply,
    Attack,
    Support
}

public enum EdgeType
{
    Concave,
    Convex,
    None
}
public enum EdgePosition
{
    Top,
    Right,
    Bottom,
    Left
}
