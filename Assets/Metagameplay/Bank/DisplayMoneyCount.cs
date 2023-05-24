using TMPro;
using UnityEngine;
using Zenject;

public class DisplayMoneyCount : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyDisplayText;
    
    private Bank _bank;
    
    [Inject]
    private void Construct(Bank bank)
    {
        _bank = bank;
    }

    private void Awake()
    {
        moneyDisplayText.text = _bank.Money + "";
    }

    private void OnEnable()
    {
        _bank.OnMoneyCountChangedEvent += MoneyCountChanged;
    }

    private void OnDisable()
    {
        _bank.OnMoneyCountChangedEvent -= MoneyCountChanged;
    }

    private void MoneyCountChanged(int count)
    {
        moneyDisplayText.text = count + "";
    }
}
