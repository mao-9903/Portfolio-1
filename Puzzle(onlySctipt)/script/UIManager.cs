using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI moneyText;

    private void OnEnable()
    {
        GameManager.OnHPChanged += UpdateHPText;
        GameManager.OnMoneyChanged += UpdateMoneyText;
    }

    private  void OnDisable()
    {
        GameManager.OnHPChanged -= UpdateHPText;
        GameManager.OnMoneyChanged -= UpdateMoneyText;
    }

    private void UpdateHPText(int newHP){
        hpText.text = "HP: " + newHP.ToString();
    }

    private void UpdateMoneyText(int newMoney){
        moneyText.text = "Money: " + newMoney.ToString();
    }
}
