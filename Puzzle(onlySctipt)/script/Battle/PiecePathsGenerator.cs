using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PiecePathsGenerator
{
    internal List<List<GameObject>> pieceQueue;
    private PuzzleFieldManager puzzleFieldManager;
    private List<GameObject> stillPassedPieces;

    public PiecePathsGenerator(){
        puzzleFieldManager = GameObject.Find("PuzzleField").GetComponent<PuzzleFieldManager>();
        pieceQueue = new List<List<GameObject>>();
        stillPassedPieces = new List<GameObject>();
    }
    internal void GenerateQueue(){
        List<List<GameObject>> paths = new List<List<GameObject>>();
        List<GameObject> initialPath = new List<GameObject>();
        GameObject manaSupply = GetManaSupply();
        if(manaSupply == null){return;}
        initialPath.Add(manaSupply);
        paths.Add(initialPath);

        while(paths.Count > 0){
            List<GameObject> currentPath = paths[paths.Count - 1];
            paths.RemoveAt(paths.Count - 1);

            GameObject currentObj = currentPath[currentPath.Count - 1];
            List<GameObject> nextPieces = GetNextPieces(currentObj);
            if(nextPieces.Count == 0){
                pieceQueue.Add(currentPath);
            } else {
                foreach(GameObject piece in nextPieces){
                    List<GameObject> newPath = new List<GameObject>(currentPath){piece};
                    stillPassedPieces.Add(piece);
                    paths.Add(newPath);
                }
            }
        }
    }

    private List<GameObject> GetNextPieces(GameObject pieceObj){
        PuzzlePiece piece = pieceObj.GetComponent<PuzzlePiece>();
        List<GameObject> nextPieceObjs = new List<GameObject>();
        foreach(var value in piece.EdgeFilled.Values){
            if(value != null && !stillPassedPieces.Contains(value)){
                nextPieceObjs.Add(value);
            }
        }
        return nextPieceObjs;
    }

    private GameObject GetManaSupply(){
        foreach(var pieceObj in puzzleFieldManager.PiecesInField){
            PuzzlePiece piece = pieceObj.GetComponent<PuzzlePiece>();
            if(piece.Type == PuzzlePieceType.ManaSupply){return pieceObj;}
        }
        return null;
    }

    internal bool IsAllPassed(){
        bool allPieceIsPassed = puzzleFieldManager.PiecesInField.All(elements => stillPassedPieces.Contains(elements));
        return allPieceIsPassed;

    }
}
