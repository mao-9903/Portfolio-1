using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敵キャラ一体の体力管理，Effectの数量管理及びそのUIへの反映に責任
public class EnemyStatus : MonoBehaviour
{
    private int _maxHP;
    private int _hp;
    internal int MaxHP{
        get => _maxHP;
        set{
            if(_maxHP != value){
                _maxHP = value;
                UpdateUI();
            }
        }
    }
    internal int HP{
        get => _hp;
        set{
            if(_hp != value){
                _hp = value;
                if(_hp > MaxHP){
                    _hp = MaxHP;
                }
                UpdateUI();
            }
        }
    }

    public EnemyStatus(int maxHP)
    {
        MaxHP = maxHP;
        HP = maxHP;
    }

    //敵キャラに関するUIを変更するメソッド
    private void UpdateUI(){}
    
    //Effect
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
