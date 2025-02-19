using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManage : MonoBehaviour
{
	private void OnEnable()
	{
		GameSystem.instance.LoadItemData();
	}
}
