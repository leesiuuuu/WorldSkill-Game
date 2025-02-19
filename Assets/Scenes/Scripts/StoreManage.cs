using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManage : MonoBehaviour
{
	public GameManage gm;
	private void OnEnable()
	{
		Time.timeScale = 0f;
		GameSystem.instance.LoadItemData();
	}
	public void StoreDone()
	{
		Time.timeScale = 1f;
		gm.CloseStore();
	}
}
