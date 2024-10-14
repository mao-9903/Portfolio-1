using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "ScriptableHealthData", menuName = "ScriptableHealthData", order = 0)]
public class ScriptableHealthData : ScriptableObject {
    public string enemyName;
    public int maxHealth;
}

