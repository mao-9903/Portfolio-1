using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Effectの個数とUIの表示に責任を持つクラス
public class PlayerStatus : MonoBehaviour
{
    //UIの更新メソッド
    private void UpdateUI(){

    }
    private int _power;
    internal int Power{
        get => _power;
        set{
            if(_power != value){
                _power = value;
                UpdateUI();
            }
        }
    }

    private int _shield;
    internal int Shield{
        get => _shield;
        set{
            if(_shield != value){
                _shield = value;
                UpdateUI();
            }
        }
    }

    
}
