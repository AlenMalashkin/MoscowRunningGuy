using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Item : MonoBehaviour
{
    private ItemState _state;
    [SerializeField] private string itemName;
    [SerializeField] private Button equipOrBuyButton;
    [SerializeField] private TextMeshProUGUI equipOrBuyText;
    [SerializeField] private int cost;
    
    private Bank _bank;

    [Inject]
    private void Construct(Bank bank)
    {
        _bank = bank;
    }

    private void Awake()
    {
        DefineItemState();
    }

    private void OnEnable()
    {
        equipOrBuyButton.onClick.AddListener(EquipOrBuy);
    }

    private void OnDisable()
    {
        equipOrBuyButton.onClick.RemoveListener(EquipOrBuy);
    }

    private void DefineItemState()
    {
        if (PlayerPrefs.HasKey(itemName))
        {
            _state = ItemState.Bought;
            equipOrBuyText.text = "Куплено";
        }
        else
        {
            _state = ItemState.NotBought;
            equipOrBuyText.text = "Купить";
        }

        if (PlayerPrefs.GetString("Skin", "Default") == itemName)
        {
            _state = ItemState.Equipped;
            equipOrBuyButton.interactable = false;
            equipOrBuyText.text = "Экипировано";
        }
    }

    private void EquipOrBuy()
    {
        switch (_state)
        {
            case ItemState.Bought:
                Equip();
                break;
            case ItemState.NotBought:
                Buy();
                break;
        }
    }

    private void Buy()
    {
        if (_bank.SpendMoney(cost))
        {
            _state = ItemState.Bought;
            PlayerPrefs.SetString(itemName, "Bought");
            equipOrBuyText.text = "Куплено";
        }
    }

    private void Equip()
    {
        _state = ItemState.Equipped;
        PlayerPrefs.SetString("Skin", itemName);
        equipOrBuyButton.interactable = false;
        equipOrBuyText.text = "Экипировано";
    }
}
