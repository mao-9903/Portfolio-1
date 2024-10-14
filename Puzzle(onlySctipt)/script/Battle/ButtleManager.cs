using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtleManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    private GameObject enemy;
    void Start(){
        EnemyGenerator enemyGenerator = new EnemyGenerator();
        switch(gameManager.gameProgress.Layer){
            case 1:
                enemyGenerator.enemyPool = new FirstEnemyPool();
                break;
            case 2:
                enemyGenerator.enemyPool = new SecondEnemyPool();
                break;
            case 3:
                enemyGenerator.enemyPool = new ThirdEnemyPool();
                break;
        }
        if(gameManager.gameProgress.battleCount<3){
            enemy =  enemyGenerator.generateEnemy(enemyGenerator.enemyPool.weakPool);
        } else {
            enemy = enemyGenerator.generateEnemy(enemyGenerator.enemyPool.strongPool);
        }
        
        SetTargetEnemy();
    }

    private void SetTargetEnemy(){
        PuzzlePiece.enemyStatus = enemy.GetComponent<EnemyStatus>();
    }

    private void EndBattle(){
        gameManager.gameProgress.battleCount += 1;
        RNGManager.UpdateState(RNGManager.RNGType.EnemyGenerate);
        RNGManager.SaveState();
    }
}