using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManage : MonoBehaviour
{
	public GameManage gm;

	public WheelItemMove wim;
	public EngineItemMove eim;
	public TransmissionItemMove tim;
	private void OnEnable()
	{
		Time.timeScale = 0f;

		wim.LoadIndex();
		eim.LoadIndex();
		tim.LoadIndex();

		GameSystem.instance.LoadItemData();
		GameSystem.instance.LoadStoreData();
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

	}
}
