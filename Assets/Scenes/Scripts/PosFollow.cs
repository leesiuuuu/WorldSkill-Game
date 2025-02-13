using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PosFollow : MonoBehaviour
{
    public GameObject Player;

	private void FixedUpdate()
	{
        if (Player == null) return;

        transform.position = Player.transform.position;
	}
}
