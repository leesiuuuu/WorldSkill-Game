using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinEffect : MonoBehaviour
{
	private Rigidbody rb;
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.AddForce(Vector3.up * 50, ForceMode.Impulse);
		Destroy(gameObject, 1f);
	}
	private void Update()
	{
		rb.AddForce(Vector3.down * 10, ForceMode.Acceleration);
		Vector3 rotation = transform.eulerAngles;
		rotation.y += Time.deltaTime * 700f;
		transform.eulerAngles = rotation;
	}
}
