using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;

public class PuzzlePieceLoadAndSaver {
    private static string filePath;

    internal List<PuzzlePiece> puzzlePieces = new List<PuzzlePiece>();
    internal List<GameObject> PieceObjList = new List<GameObject>();
    
    
    public PuzzlePieceLoadAndSaver()
    {
        
        filePath = Path.Combine(Application.persistentDataPath, "puzzlePieces.json");
        Debug.Log("Load and Saver is generated!");
    }
    
    public void SavePuzzlePieses(){
        var datas = puzzlePieces.Select(piece => new PuzzlePieceData{
            UsageCount = piece.UsageCount,
            pieceClass = piece.GetType().Name // クラス名を保存
        }).ToList();

        string json = JsonUtility.ToJson(new SerializationWrapper<PuzzlePieceData>(datas));
        File.WriteAllText(filePath, json);
        Debug.Log("PersistentDataPath: " + Application.persistentDataPath);
    }

    public void LoadPuzzlePieces(){
        Debug.Log("try load");
        PuzzlePieceFactory factory = new PuzzlePieceFactory();
        PieceImageAttacher imageAttacher = new PieceImageAttacher();
        if(File.Exists(filePath)){
            string json = File.ReadAllText(filePath);
            var data = JsonUtility.FromJson<SerializationWrapper<PuzzlePieceData>>(json).Items;

            puzzlePieces.Clear();
            foreach(var pieceData in data){
                GameObject pieceObj = factory.LoadAndInstantiatePrefab();
                PuzzlePiece piece = factory.CreatePuzzlePiece(pieceObj, pieceData);
                Sprite sprite = imageAttacher.GetAppropriateImage(piece);
                pieceObj.GetComponent<Image>().sprite = sprite;
                puzzlePieces.Add(piece);
                PieceObjList.Add(pieceObj);
                imageAttacher.ReleaseHandle();
            }
        }
        factory.ReleaseResources();
    }

    
    private void OnApprivationQuit(){
        //後で作成
    }
}