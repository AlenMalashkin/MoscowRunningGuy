using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ShopWindow : MonoBehaviour
{
    public event Action<int> OnItemChangedEvent;
    
    [SerializeField] private List<Item> items;
    [SerializeField] private Transform itemContainer;
    [SerializeField] private Button nextItem;
    [SerializeField] private Button prevItem;

    private DiContainer _diContainer;
    private int _currentItem = 0;

    [Inject]
    private void Construct(DiContainer diContainer)
    {
        _diContainer = diContainer;
    }
    
    private void Awake()
    {
        PlayerPrefs.SetString("Default", "Bought");
        
        _diContainer.InstantiatePrefabForComponent<Item>(items[_currentItem], itemContainer);

        if (_currentItem == 0)
            prevItem.interactable = false;
        
        if (_currentItem == items.Count - 1)
            nextItem.interactable = false;
    }

    private void OnEnable()
    {
        OnItemChangedEvent += OnItemChanged;
        nextItem.onClick.AddListener(NextItem);
        prevItem.onClick.AddListener(PrevItem);
    }

    private void OnDisable()
    {
        OnItemChangedEvent -= OnItemChanged;
        nextItem.onClick.RemoveListener(NextItem);
        prevItem.onClick.RemoveListener(PrevItem);
    }

    private void NextItem()
    {
        _currentItem++;
        RenderItem();
        OnItemChangedEvent?.Invoke(_currentItem);
    }

    private void PrevItem()
    {
        _currentItem--;
        RenderItem();
        OnItemChangedEvent?.Invoke(_currentItem);
    }

    private void RenderItem()
    {
        Destroy(itemContainer.GetChild(0).gameObject);

        _diContainer.InstantiatePrefabForComponent<Item>(items[_currentItem], itemContainer);
    }

    private void OnItemChanged(int itemId)
    {
        if (itemId == 0)
            prevItem.interactable = false;

        if (itemId == items.Count - 1)
            nextItem.interactable = false;

        if (itemId > 0)
            prevItem.interactable = true;

        if (itemId < items.Count - 1)
            nextItem.interactable = true;
    }
}
