using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class RNGManager 
{
    public static string path =Path.Combine(Application.persistentDataPath, "RNG.json");
    public enum RNGType{
        MapGeneration,
        ItemDrop,
        EnemyGenerate
    }

    //各RNGTypeに対応する乱数Stateを保持するディクショナリ
    private static Dictionary<RNGType, UnityEngine.Random.State> rngStates = new Dictionary<RNGType, UnityEngine.Random.State>();

    //乱数生成器の初期化
    internal static void Initialize(){
        foreach(RNGType type in Enum.GetValues(typeof(RNGType))){
            rngStates[type] = UnityEngine.Random.state;
        }
    }
    //指定したStateから乱数を取得メソッド
    internal static int GetRandomInt(RNGType type, int minValue, int maxValue){
        UnityEngine.Random.State originalState = UnityEngine.Random.state;      //現在のStateを保持
        UnityEngine.Random.state = rngStates[type];     //指定したStateを取得
        int result = UnityEngine.Random.Range(minValue,maxValue);   //乱数を生成
        UnityEngine.Random.state = originalState;   //元のStateに戻す
        return result;
    }

    //指定したRNGStateを更新するメソッド
    internal static void UpdateState(RNGType type){
        UnityEngine.Random.State originalState = UnityEngine.Random.state;//現在のStateの保持
        UnityEngine.Random.state = rngStates[type];     //指定したStateを使用
        UnityEngine.Random.Range(0,100);    //乱数を生成しStateを更新
        rngStates[type] = UnityEngine.Random.state; //更新したStateの保存
        UnityEngine.Random.state = originalState;
    }

    //Json形式からStateをロードするメソッド
    internal static void SaveState(){
        var stateData = new Dictionary<string, string>();//シリアライズ可能な文字列形式に変更
        foreach(var entry in rngStates){
            stateData[entry.Key.ToString()] = JsonUtility.ToJson(entry.Value);
        }

        //json形式で保存
        string json = JsonUtility.ToJson(new RNGStateContainer(stateData));
        File.WriteAllText(path, json);
    }

    //Json形式からStateをロードするメソッド
    internal static void LoadState(){
        if(File.Exists(path)){
            string json = File.ReadAllText(path);
            var container = JsonUtility.FromJson<RNGStateContainer>(json);  //jsonをデシリアライズ
            //Stateを復元
            foreach(var entry in container.StateData){
                RNGType type;
                if(Enum.TryParse(entry.Key, out type)){
                    rngStates[type] = JsonUtility.FromJson<UnityEngine.Random.State>(entry.Value);
                }
            }
        } else {
            Initialize();
        }
        
    }

    //Stateのシリアライズ用のクラス
    [Serializable]
    private class RNGStateContainer{
        public Dictionary<string, string> StateData;
        public RNGStateContainer(Dictionary<string,string> stateData){
            StateData = stateData;
        }
    }
}
