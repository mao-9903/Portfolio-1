using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameProgress
{
    internal int Layer;
    internal int nodeId;
    internal int battleCount;
}

[System.Serializable]
public class PlayerData{
    internal int currentHP;
    internal int maxHP;
    internal List<string> inventoryItem;
    internal List<string> Artifacts;
}

[System.Serializable]
public class SaveData{
    internal GameProgress gameProgress;
    internal PlayerData playerData;
}

public class SaveAndLoader{
    internal void Save(){
        SaveData saveData = new SaveData();

    }
    internal void Load(){

    }
}