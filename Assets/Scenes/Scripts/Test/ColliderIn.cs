using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderIn : MonoBehaviour
{
	public GameManage GameManage;
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (gameObject.CompareTag("In"))
			{
				GameManage.GameStart = true;
			}
			else if(gameObject.CompareTag("Out"))
			{
				GameManage.GameOver();
			}
		}
		if (other.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Out"))
		{
			GameManage.PlayerWin = false;
		}
	}
}
