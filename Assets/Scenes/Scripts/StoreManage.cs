using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManage : MonoBehaviour
{
	public GameManage gm;

	public WheelItemMove wim;
	public EngineItemMove eim;
	public TransmissionItemMove tim;

	public GameObject player;

	public GameObject CoinEffect;

	public Text MoneyUI;
	private void OnEnable()
	{
		Time.timeScale = 0f;

		MoneyUI.text = "Money : " + GameSystem.instance.Money. ToString() + "$";

		wim.LoadIndex();
		eim.LoadIndex();
		tim.LoadIndex();

		GameSystem.instance.LoadItemData();
		GameSystem.instance.LoadStoreData();

		wim.TitleInfo();
	}
	public void UpdateMoneyUI()
	{
		MoneyUI.text = "Money : " + GameSystem.instance.Money.ToString() + "$";
	}
	public void StoreDone()
	{
		Time.timeScale = 1f;

		wim.SaveIndex();
		eim.SaveIndex();
		tim.SaveIndex();

		GameSystem.instance.SaveItemData();
		GameSystem.instance.SaveStoreData();

		gm.CloseStore();
	}

	public void ItemRespond(string name)
	{
		switch (name)
		{
			case "100$":
				GameSystem.instance.Money += 100;
				GameSystem.instance.SaveItemData();
				Debug.Log("현재 돈 : " + GameSystem.instance.Money);
				break;
			case "5000$":
				GameSystem.instance.Money += 5000;
				GameSystem.instance.SaveItemData();
				Debug.Log("현재 돈 : " + GameSystem.instance.Money);
				break;
			case "1000$":
				GameSystem.instance.Money += 1000;
				GameSystem.instance.SaveItemData();
				Debug.Log("현재 돈 : " + GameSystem.instance.Money);
				break;
			case "속도 소폭 증가":
				player.GetComponent<KartController>().MoveSpeed += 20;
				Invoke("MoveSpeedReset", 1f);
				break;
			case "속도 대폭 증가":
				player.GetComponent<KartController>().MoveSpeed += 40;
				Invoke("MoveSpeedReset", 1f);
				break;
		}
	}
	private void MoveSpeedReset()
	{
		player.GetComponent<KartController>().MoveSpeed = 100;
	}
}
