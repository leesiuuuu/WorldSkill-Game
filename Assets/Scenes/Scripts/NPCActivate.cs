using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCActivate : MonoBehaviour
{
	public GameObject[] cars;
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			StartCoroutine(nigga());
		}
	}
	IEnumerator nigga()
	{
		foreach (GameObject car in cars)
		{
			car.SetActive(true);
			yield return new WaitForSeconds(Random.Range(0.4f, 1f));
		}
	}
}
