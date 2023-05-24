using System;
using UnityEngine;

public class Bank
{
	public event Action<int> OnMoneyCountChangedEvent;

	public int Money { get; private set; }

	public Bank()
	{
		Money = PlayerPrefs.GetInt("Money", 0);
	}

	public void GetMoney(int amount)
	{
		Money += amount;
		PlayerPrefs.SetInt("Money", Money);
		OnMoneyCountChangedEvent?.Invoke(Money);
	}

	public bool SpendMoney(int amount)
	{
		if (Money >= amount)
		{
			Money -= amount;
			PlayerPrefs.SetInt("Money", Money);
			OnMoneyCountChangedEvent?.Invoke(Money);
			return true;
		}
		
		return false;
	}
}
