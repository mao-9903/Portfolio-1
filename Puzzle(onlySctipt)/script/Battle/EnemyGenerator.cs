using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class EnemyGenerator 
{
    internal EnemyPool enemyPool;
    
    internal GameObject generateEnemy(List<string> pool){
        string EnemyName = LotteryEnemy(pool);
        GameObject Instance = InstantiatePrefab();
        SetStatus(EnemyName,Instance);
        SetImage(EnemyName, Instance);
        return Instance;
    }

    //リストから敵名を取得
    private string LotteryEnemy(List<string> list){
        int randInt = RNGManager.GetRandomInt(RNGManager.RNGType.EnemyGenerate, 0, list.Count);
        
        string EnemyName = list[randInt];
        Debug.Log(EnemyName);
        return EnemyName;
    }

    //EnemyPrefabのインスタンスを生成
    private GameObject InstantiatePrefab(){
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>("EnemyPrefab");
        handle.WaitForCompletion();
        
        GameObject obj = GameObject.Instantiate(handle.Result);
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        obj.transform.localPosition = new Vector3(-500,200,0);
        Addressables.Release(handle);
        return obj;
    }

    //敵の種類に応じて体力を初期化
    private void SetStatus(string name, GameObject obj){
        EnemyStatus status = obj.GetComponent<EnemyStatus>();
        string address = "EnemyStatus/" + name;
        AsyncOperationHandle<ScriptableHealthData> handle = Addressables.LoadAssetAsync<ScriptableHealthData>(address);
        handle.WaitForCompletion();
        ScriptableHealthData healthData = handle.Result;
        status.MaxHP = healthData.maxHealth;
        status.HP = healthData.maxHealth;
        Addressables.Release(handle);
        
    }

    // 画像を設定
    private void SetImage(string name, GameObject obj){
        Image image = obj.GetComponent<Image>();
        string spriteAddress = "EnemySprites/" + name;
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(spriteAddress);
        // 以下非同期での処理
        handle.Completed += operation =>{
            if(operation.Status == AsyncOperationStatus.Succeeded){
                image.sprite = handle.Result;
            } else {
                Debug.Log("Fail to Load Sprite");
            }
            Addressables.Release(handle);
        };
    }
    
}

public abstract class EnemyPool{
    internal List<string> weakPool;
    internal List<string> strongPool;
}

public class FirstEnemyPool : EnemyPool{
    public FirstEnemyPool(){
        weakPool = new List<string>(){
            "Goblin",
            // "Hornet","Wolf"
        };
        strongPool = new List<string>(){
            "Orc", "Harpy"
        };
    }
}

public class SecondEnemyPool : EnemyPool{
    public SecondEnemyPool(){
        weakPool = new List<string>(){

        };
        strongPool = new List<string>(){

        };
    }
}
public class ThirdEnemyPool : EnemyPool{
    public ThirdEnemyPool(){
        weakPool = new List<string>(){

        };
        strongPool = new List<string>(){

        };
    }
}
