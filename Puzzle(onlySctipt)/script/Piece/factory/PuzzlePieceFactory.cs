using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using UnityEngine.UI;



public class PuzzlePieceFactory 
{
    private string Address = "PiecePrefab"; //ベースとなるPieceのプレファブのpath
    private GameObject cachedPrefab = null; //上記のプレファブ  
    
    //DataクラスからPieceを復元するクラス
    internal PuzzlePiece  CreatePuzzlePiece(GameObject obj, PuzzlePieceData data)
    {
        Type pieceType = Type.GetType(data.pieceClass);

        if (pieceType == null)
        {
            throw new System.ArgumentException($"Unknown piece class: {data.pieceClass}");
        }

        PuzzlePiece piece = (PuzzlePiece)obj.AddComponent(pieceType);
        

        piece.Initialize(data.UsageCount);
        return piece;
    }

    //プレファブのインスタンス化
    internal GameObject LoadAndInstantiatePrefab(Transform parent = null){
        GameObject instance;
        //必要に応じて同期的にロード
        if (cachedPrefab == null){
            cachedPrefab = Addressables.LoadAssetAsync<GameObject>(Address).WaitForCompletion();
        }
        instance = GameObject.Instantiate(cachedPrefab, parent);
        return instance;
    }

    private void AttachPieceImage(GameObject obj, PuzzlePiece piece){

        
    }

    //必要に応じてプレファブを明示的にリリース
    internal void ReleaseResources(){
        if(cachedPrefab != null){
            Addressables.Release(cachedPrefab);
            cachedPrefab = null;
        }
    }
    
}

public class PieceImageAttacher{
    internal AsyncOperationHandle<Sprite> handle;
    internal Sprite GetAppropriateImage(PuzzlePiece piece){
        Sprite sprite = null;
        Dictionary<EdgePosition, EdgeType> edges = piece.Edges;
        if(edges[EdgePosition.Top] == EdgeType.Convex){
            if(edges[EdgePosition.Bottom] == EdgeType.Convex){
                string path = "Piece/(-1,0,-1,0)";
                handle = LoadAddressableSprite(path);
                sprite = handle.Result;
                return sprite;
            } else if(edges[EdgePosition.Bottom] == EdgeType.None){
                string path = "Pieces/(1,0,0,0)";
                handle = LoadAddressableSprite(path);
                sprite = handle.Result;
                return sprite;
            } else if(edges[EdgePosition.Bottom] == EdgeType.Concave){
                if(edges[EdgePosition.Right] == EdgeType.Convex){
                    string path = "Pieces/(1,1,-1,0)";
                    handle = LoadAddressableSprite(path);
                    sprite = handle.Result;
                    return sprite;
                } else if(edges[EdgePosition.Right] == EdgeType.None){
                    string path = "Pieces/(1,0,-1,0)";
                    handle = LoadAddressableSprite(path);
                    sprite = handle.Result;
                    return sprite;
                } else if(edges[EdgePosition.Right] == EdgeType.Concave){
                    string path = "Pieces/(1,-1,-1,0)";
                    handle = LoadAddressableSprite(path);
                    sprite = handle.Result;
                    return sprite;
                }
            }
        } else if(edges[EdgePosition.Top] == EdgeType.None){
            if(edges[EdgePosition.Bottom] == EdgeType.Convex){
                string path = "Pieces/(0,0,1,0)";
                handle = LoadAddressableSprite(path);
                sprite = handle.Result;
                return sprite;
            } else if(edges[EdgePosition.Bottom] == EdgeType.Concave){
                string path = "Pieces/(0,0,-1,0)";
                handle = LoadAddressableSprite(path);
                sprite = handle.Result;
                return sprite;
            }
        } else if(edges[EdgePosition.Top] == EdgeType.Concave){
            if(edges[EdgePosition.Bottom] == EdgeType.None){
                string path = "Pieces/(-1,0,0,0)";
                handle = LoadAddressableSprite(path);
                sprite = handle.Result;
                return sprite;
            } else if(edges[EdgePosition.Bottom] == EdgeType.Concave){
                string path = "Piece/(-1,0,-1,0)";
                handle = LoadAddressableSprite(path);
                sprite = handle.Result;
                return sprite;
            }
        }

        return null;
    }

    AsyncOperationHandle<Sprite> LoadAddressableSprite(string path){
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(path);
        handle.WaitForCompletion();
        return handle;
    }
    internal void ReleaseHandle(){
        Addressables.Release(handle);
    }
}
