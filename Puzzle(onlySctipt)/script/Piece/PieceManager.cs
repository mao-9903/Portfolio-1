using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    PuzzlePieceLoadAndSaver LoadAndSaver;
    [SerializeField]
    private Transform Inventory;
    [SerializeField]
    private Transform HiddenField;

    void Awake()
    {
        LoadAndSaver = new PuzzlePieceLoadAndSaver();
        
    }
    void Start()
    {
        LoadAndSaver.LoadPuzzlePieces();
        // #if UNITY_EDITOR
        //     foreach(Transform child in Inventory){
        //         LoadAndSaver.puzzlePieces.Add(child.GetComponent<PuzzlePiece>());
        //         Debug.Log(child.name);
        //     }
        // #endif
        foreach (GameObject obj in LoadAndSaver.PieceObjList){
            if(Inventory.childCount < 4){
                obj.transform.SetParent(Inventory);
            } else {
                obj.transform.SetParent(HiddenField);
                obj.transform.localPosition = new Vector3(0,0,0);
            }
            
        }
    }

    private void OnDestroy() {
        LoadAndSaver.SavePuzzlePieses();
        Debug.Log("Save");
    }
}
