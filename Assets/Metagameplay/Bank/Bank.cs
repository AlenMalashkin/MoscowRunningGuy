using System;
using System.Data;
using UnityEngine;

public class Bank
{
	public event Action<int> OnMoneyCountChangedEvent;

	public int Money { get; private set; }

	public Bank()
	{
		string money = Db.ExecuteQueryWithAnswer("SELECT Money FROM Player WHERE Id = 1");

		Money = int.Parse(money);
	}

	public void GetMoney(int amount)
	{
		Money += amount;
		Db.ExecuteQueryWithoutAnswer($"UPDATE Player SET Money = {Money} WHERE Id = 1");
		OnMoneyCountChangedEvent?.Invoke(Money);
	}

	public bool SpendMoney(int amount)
	{
		if (Money >= amount)
		{
			Money -= amount;
			Db.ExecuteQueryWithoutAnswer($"UPDATE Player SET Money = {Money} WHERE Id = 1");
			OnMoneyCountChangedEvent?.Invoke(Money);
			return true;
		}
		
		return false;
	}
}
