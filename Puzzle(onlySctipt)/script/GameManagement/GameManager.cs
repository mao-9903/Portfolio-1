using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    internal static GameManager Instance{get; private set;}
    internal GameProgress gameProgress;
    internal static event Action<int> OnHPChanged;
    internal static event Action<int> OnMoneyChanged;

    private void Awake(){
        if(Instance==null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        gameProgress = new GameProgress();
        #if UNITY_EDITOR
            gameProgress.Layer = 1;
            gameProgress.battleCount = 0;
        #endif

        RNGManager.LoadState();
    }

    private int _playerHP;
    internal int PlayerHP{
        get{ return _playerHP; }
        set{
            if(_playerHP!=value){
                _playerHP = value;
                OnHPChanged?.Invoke(_playerHP);
            }
        }
    }

    private int _money;
    internal int Money{
        get{ return _money; }
        set{
            if(_money != value){
                _money = value;
                OnMoneyChanged?.Invoke(_money);
            }
        }
    }

    
    public void SaveData(){
        PlayerPrefs.SetInt("PlayerHP",_playerHP);
        PlayerPrefs.SetInt("Money",_money);
        PlayerPrefs.Save();
        RNGManager.SaveState();
    }
    public void LoadGame()
    {
        _playerHP = PlayerPrefs.GetInt("PlayerHP", 100); // デフォルト値
        _money = PlayerPrefs.GetInt("PlayerMoney", 0); // デフォルト値

        // イベントの発行（初期化時にも通知を送る）
        OnHPChanged?.Invoke(_playerHP);
        OnMoneyChanged?.Invoke(_money);
    }
}
