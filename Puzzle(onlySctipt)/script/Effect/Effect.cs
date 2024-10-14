using UnityEngine;

//敵へダメージを与える汎用クラス
public class DamageToEnemy{
    internal EnemyStatus target;
    private int damageAmount;
    public DamageToEnemy(EnemyStatus target,int amount){
        this.target = target;
        this.damageAmount = amount;
    }

    //シールドを削りながらダメージを与えるメソッド
    internal void ApplyDamageWithShield(){
        EnemyEffectModifier enemyEffectModifier = new EnemyEffectModifier(target,EffectType.Shield, damageAmount);
        int remainDamage = damageAmount - target.Shield;
        if(remainDamage < 0){
            remainDamage = 0;
        }
        target.HP -= remainDamage;
    }
    //シールドを無視してダメージを与えるメソッド
    internal void ApplyDamageDirectry(){
        target.HP -= damageAmount;
    }
}

//Playerにダメージを与える汎用クラス
public class DamageToPlayer{
    private int damageAmount;
    
    public DamageToPlayer(int amount){
        damageAmount = amount;
    }

    //シールドを消費し，超過分を受ける
    internal void ApplyDamageWithShield(){
        PlayerEffectModifier playerEffectModifier = new PlayerEffectModifier(EffectType.Shield, -damageAmount);
        int remainDamage = damageAmount - playerEffectModifier.playerStatus.Shield;
        if(remainDamage <0){
            remainDamage = 0;
        }
        playerEffectModifier.ApplyEffect();
        if(GameManager.Instance!=null){
            GameManager.Instance.PlayerHP -= remainDamage;
        }
    }

    //非戦闘シーンでの利用
    internal void ApplyDamageDirectry(){
        if(GameManager.Instance!=null){
            GameManager.Instance.PlayerHP -= damageAmount;
        }
    }
}

//戦闘シーンのみ,PlayerにEffectを付与する汎用クラス
public class PlayerEffectModifier{
    internal PlayerStatus playerStatus;
    private EffectType effectType;
    private int effectAmount;
    public PlayerEffectModifier(EffectType effect, int num){
        this.effectType = effect;
        this.effectAmount = num;
    }
    internal void ApplyEffect(){
        switch(effectType){
            case EffectType.Power:
                playerStatus.Power += effectAmount;
                break;
            case EffectType.Shield:
                playerStatus.Shield += effectAmount;
                if(playerStatus.Shield < 0){
                    playerStatus.Shield = 0;
                }
                break;
            
        }
    }
    internal void GetPlayer(){
        //playerStatusを取得するメソッド
        playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
    }
}

//戦闘中のみ　敵キャラにEffectを与える汎用クラス
public class EnemyEffectModifier{
    private EnemyStatus target;
    private EffectType effectType;
    private int effectAmount;
    public EnemyEffectModifier(EnemyStatus target, EffectType effect, int amount){
        this.target = target;
        this.effectAmount = amount;
        this.effectType = effect;
    }
    private void ApplyEffect(){
        switch (effectType){
            case EffectType.Power:
                target.Power += effectAmount;
                break;
            case EffectType.Shield:
                target.Shield += effectAmount;
                break;
        }
    }
}

//戦闘中に用いるEffectの種類の列挙体
public enum EffectType{
    Power,
    Shield
}

public enum Attribute{
    None,
    Fire,
    Water,
    Wind,
    Earth,
    Holy,
    Darkness

}
