using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public GameObject Player;
    public float CameraSpeed = 10f;
    public float distance = 10f;
    public float Height = 5f;
    public float RotateSpeed;

	private void FixedUpdate()
	{
        if (Player == null) return;

        Vector3 forward = Player.transform.forward;

        Vector3 targetPos = Player.transform.position - forward * distance + Vector3.up * Height;

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * CameraSpeed);

        Quaternion targetRot = Quaternion.LookRotation(Player.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * RotateSpeed);
	}
}
